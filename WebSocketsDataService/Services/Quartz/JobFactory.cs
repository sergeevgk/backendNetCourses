using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Quartz
{
	public class JobFactory : IJobFactory
	{
		private readonly IServiceProvider provider;

		public JobFactory(IServiceProvider provider)
		{
			this.provider = provider;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			return new JobWrapper(provider, bundle.JobDetail.JobType);
		}

		public void ReturnJob(IJob job)
		{
			(job as IDisposable)?.Dispose();
		}
	}
	public class JobWrapper : IJob, IDisposable
	{
		private readonly IServiceScope serviceScope;
		private readonly IJob job;

		public JobWrapper(IServiceProvider serviceProvider, Type jobType)
		{
			serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			job = ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, jobType) as IJob;
		}

		public Task Execute(IJobExecutionContext context)
		{
			return job.Execute(context);
		}

		public void Dispose()
		{
			serviceScope.Dispose();
		}
	}
}
