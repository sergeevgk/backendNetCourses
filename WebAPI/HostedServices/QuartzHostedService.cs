using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Quartz;

namespace WebAPI.HostedServices
{
	public class QuartzHostedService : BackgroundService
	{
		private readonly ILogger<QuartzHostedService> logger;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly IJobFactory jobFactory;

		public QuartzHostedService(ILogger<QuartzHostedService> logger,
								IServiceScopeFactory serviceScopeFactory,
								IJobFactory jobFactory)
		{
			this.logger = logger;
			this.serviceScopeFactory = serviceScopeFactory;
			this.jobFactory = jobFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
				scheduler.JobFactory = jobFactory;
				await scheduler.Start();

				IJobDetail jobDetail = JobBuilder.Create<UnitEventsJob>().Build();
				ITrigger trigger = TriggerBuilder.Create()
					.WithIdentity("UnitsSynchronizationEvent", "default")
					.StartNow()
					.WithSimpleSchedule(x => x
					.WithIntervalInMinutes(1)
					.RepeatForever())
					.Build();

				await scheduler.ScheduleJob(jobDetail, trigger);
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
