using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebSocketsDataService.Hubs;
using WebSocketsDataService.Quartz;

namespace WebSocketsDataService.HostedServices
{
	public class QuartzHostedService : BackgroundService
	{
		private readonly ILogger<QuartzHostedService> logger;
		private readonly IConfiguration configuration;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly IServiceProvider serviceProvider;
		private readonly IJobFactory jobFactory;

		public QuartzHostedService(ILogger<QuartzHostedService> logger,
								IConfiguration configuration,
								IServiceScopeFactory serviceScopeFactory,
								IServiceProvider serviceProvider,
								IJobFactory jobFactory)
		{
			this.logger = logger;
			this.configuration = configuration;
			this.serviceScopeFactory = serviceScopeFactory;
			this.serviceProvider = serviceProvider;
			this.jobFactory = jobFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
				scheduler.JobFactory = jobFactory;
				await scheduler.Start();

				var hubTypes = Assembly.GetAssembly(GetType())
				.GetTypes()
				.Where(t => !t.IsAbstract)
				.Where(t => t.IsAssignableTo(typeof(BaseHub)));

				foreach (var hubType in hubTypes)
				{
					if (serviceProvider.GetService(hubType) is BaseHub channel)
					{
						Type hubNotificationType = typeof(HubNotifications<>);
						Type[] typeArgs = { hubType };
						Type genericJob = hubNotificationType.MakeGenericType(typeArgs);

						IJobDetail jobDetail = JobBuilder.Create(genericJob).Build();
						ITrigger trigger = TriggerBuilder.Create()
							.WithIdentity(hubNotificationType.Name + hubType, "default")
							.StartNow()
							.WithCronSchedule(configuration["Quartz:QuartzCronExpression"])
							.Build();
						await scheduler.ScheduleJob(jobDetail, trigger);
					}
				}

			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
