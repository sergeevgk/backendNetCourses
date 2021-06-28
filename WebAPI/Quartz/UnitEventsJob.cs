using Common.Extensions;
using DAL.DbContexts;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI.HostedServices;

namespace WebAPI.Quartz
{
	public class UnitEventsJob : IJob
	{
		private readonly ILogger<QuartzHostedService> logger;
		private readonly IConfiguration config;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly string unitEventsUri = "https://localhost:44363/api/events/keys";
		private readonly string fullEventsUri = "https://localhost:44363/api/events";
		private readonly string serviceLoginUri = "https://localhost:44307/api/User/auth";

		private readonly int maxEventCount = 100;
		private readonly int maxParallelThreads = 5;

		public UnitEventsJob(ILogger<QuartzHostedService> logger, IConfiguration config, IServiceScopeFactory serviceScopeFactory)
		{
			this.logger = logger;
			this.config = config;
			this.serviceScopeFactory = serviceScopeFactory;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			logger.LogInformation("EventUnits service is running.");
			var authToken = await LoginServiceAccount();
			if (string.IsNullOrEmpty(authToken))
			{
				logger.LogError("Cannot get authentication token");
				return;
			}
			var unitIds = config.GetSection("UnitEvents").GetSection("ActiveUnitIds").Get<int[]>();

			if (unitIds.Length == 0)
			{
				return;
			}
			IEnumerable<Func<Task>> tasks = unitIds.Select(unitId => (Func<Task>)(() => ProcessUnitAsync(unitId, authToken)));
			await tasks.RunInParallel(logger, maxParallelThreads);
		}

		private async Task<string> LoginServiceAccount()
		{
			using HttpClient httpClient = new HttpClient();
			var loginDataJson = JsonConvert.SerializeObject(new
			{
				login = "service",
				password = "pwd123"
			});
			var content = new StringContent(loginDataJson, Encoding.UTF8, "application/json");
			var responce = await httpClient.PostAsync(serviceLoginUri, content);
			if (responce.IsSuccessStatusCode)
			{
				return await responce.Content.ReadAsStringAsync();
			}
			return "";
		}

		private async Task ProcessUnitAsync(int unitId, string authToken)
		{
			int take = maxEventCount;
			var skipCount = 0;
			using HttpClient httpClient = new HttpClient();
			List<Func<Task>> tasks = new List<Func<Task>>();
			while (true)
			{
				string unitUri = unitEventsUri + $"?unitId={unitId}&take={take}&skip={skipCount}";
				httpClient.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", authToken);
				var response = await httpClient.GetAsync(unitUri);
				if (!response.IsSuccessStatusCode)
				{
					logger.LogError("Authentication token is not valid");
					return;
				}
				var responceString = await response.Content.ReadAsStringAsync();
				int[] eventIds = JsonConvert.DeserializeObject<int[]>(responceString);
				if (eventIds.Length == 0)
				{
					break;
				}
				tasks.Add(() => ProcessUnitEventsAsync(responceString, authToken));
				logger.LogInformation("GET [unitId = {unitId}] responded with " + eventIds.Aggregate("", (res, id) => res += id + " "));
				skipCount += take;
			}
			await tasks.RunInParallel(logger, maxParallelThreads);
		}
		private async Task ProcessUnitEventsAsync(string eventIds, string authToken)
		{
			using HttpClient httpClient = new HttpClient();
			string eventUri = fullEventsUri;
			var data = new System.Net.Http.StringContent(eventIds, Encoding.UTF8, "application/json");
			httpClient.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", authToken);
			var response = await httpClient.PostAsync(eventUri, data);
			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Authentication token is not valid");
				return;
			}
			var content = await response.Content.ReadAsStringAsync();
			var events = JsonConvert.DeserializeObject<IEnumerable<UnitEvent>>(content);
			await ProcessEvents(events);
		}
		private async Task ProcessEvents(IEnumerable<UnitEvent> events)
		{
			using var scope = serviceScopeFactory.CreateScope();
			var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

			foreach (var e in events)
			{
				var existEvent = appDbContext.Set<UnitEvent>().Find(e.Id);
				if (existEvent == null)
				{
					appDbContext.Set<UnitEvent>().Add(e);
				}
				if (existEvent != null && e.IsActive)
				{
					SaveUpdateInfo(appDbContext, existEvent, e);
					appDbContext.Entry<UnitEvent>(existEvent).CurrentValues.SetValues(e);
				}
				//logger.LogInformation($"Process {e.Id} event");
			}
			await appDbContext.SaveChangesAsync();
		}

		private void SaveUpdateInfo(ApplicationDbContext appDbContext, UnitEvent unitEvent, UnitEvent newUnitEvent)
		{
			var updateRecords = typeof(UnitEvent).GetProperties()
				.Select(prop =>
				{
					var oldValue = prop.GetValue(unitEvent).ToString();
					var newValue = prop.GetValue(newUnitEvent).ToString();
					if (!oldValue.Equals(newValue))
					{
						return new UnitEventUpdate
						{
							Timestamp = DateTime.Now,
							FieldName = prop.Name,
							OldValue = oldValue,
							NewValue = newValue,
							UpdatedEventId = unitEvent.Id
						};
					}
					return null;
				})
				.Where(r => r != null);
			appDbContext.Set<UnitEventUpdate>().AddRange(updateRecords);
		}
	}
}
