using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace WebSocketsClientApp.Pages.UserPages
{
	public class ChannelPageModel : PageModel
	{
		private readonly IConfiguration configuration;
		private readonly ConnectionManager connectionManager;

		public ChannelPageModel(IConfiguration configuration, ConnectionManager connectionManager)
		{
			this.configuration = configuration;
			this.connectionManager = connectionManager;
		}

		public List<string> Subscriptions { get; set; }
		public string Message { get; set; }
		public List<HubConnection> Connections { get; set; }

		[Inject]
		public IJSRuntime JSRuntime { get; set; }
		public async Task InvokeOnMessageEvent(string message)
		{
			await JSRuntime.InvokeVoidAsync("onMessageFunction", message);
		}
		public void OnGet()
		{
			FillSubscriptions();
		}

		public PartialViewResult OnGetChannelPagePartial()
		{
			var message = HttpContext.Session.GetString("CurrentMessage");
			FillSubscriptions();
			return Partial("ChannelPagePartial", message);
		}

		public async Task OnPostStartHubs()
		{
			Connections = new List<HubConnection>();
			FillSubscriptions();
			List<Func<Task>> tasks = new List<Func<Task>>();
			foreach (var hub in Subscriptions)
			{
				tasks.Add(() => StartHub(hub));
			}
			await Task.WhenAll(tasks.Select(t => Resolve(t)));
		}

		private async Task Resolve(Func<Task> task)
		{
			await task().ConfigureAwait(false);
		}

		private async Task StartHub(string hubName)
		{
			var hubUrl = configuration["ServiceUrls:HubsBaseUrl"] + hubName;
			var authToken = HttpContext.Session.GetString("JWTToken");
			await using HubConnection hubConnection = new HubConnectionBuilder().WithUrl(hubUrl, options =>
			{
				options.AccessTokenProvider = () => Task.FromResult(authToken);
			}).Build();
			connectionManager.hubConnections.Add(hubConnection);
			await hubConnection.StartAsync().ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					Console.WriteLine($"Error on connecting to hub {hubUrl}. " + task.Exception);
				}
				else
				{
					var role = User.FindFirst(ClaimTypes.Role);
					Console.WriteLine($"Successfully connected to hub {hubUrl}");
					hubConnection.On<string>("SendMessage" + role.Value, Console.WriteLine);
					Console.ReadLine();
				}
			});
		}

		private void FillSubscriptions()
		{
			var subscriptionString = HttpContext.Session.GetString("Subscriptions");
			if (!string.IsNullOrEmpty(subscriptionString))
				Subscriptions = JsonConvert.DeserializeObject<List<string>>(subscriptionString);
		}

		public async Task OnPostStopHubs()
		{
			FillSubscriptions();
			await connectionManager.ReleaseAllConnections();
		}
	}
}
