using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using WebSocketsDataService.HostedServices;
using WebSocketsDataService.Hubs;

namespace WebSocketsDataService.Quartz
{
	public class HubNotifications<THub> : IJob where THub: BaseHub
	{
		private readonly ILogger<QuartzHostedService> logger;
		private readonly IConfiguration config;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly THub hub;
		private readonly IHubContext<THub> hubContext;

		public HubNotifications(ILogger<QuartzHostedService> logger,
								IConfiguration config,
								IServiceScopeFactory serviceScopeFactory,
								THub hub,
								IHubContext<THub> hubContext)
		{
			this.logger = logger;
			this.config = config;
			this.serviceScopeFactory = serviceScopeFactory;
			this.hub = hub;
			this.hubContext = hubContext;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			logger.LogInformation("HubNotifications service is running.");
			logger.LogInformation("{0} {1} {2}", hub.Id, hub.Name, hub.Type);
			await hub.SendMessage();
		}
	}
		
}
