using DAL.DbContexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.HostedServices
{
	public class UpdateVolumeTimedService : IHostedService, IDisposable
	{
		private readonly ILogger<UpdateVolumeTimedService> logger;
		private readonly IServiceScopeFactory serviceScopeFactory;
		private readonly int delayInSeconds = 10;
		private readonly Random rnd = new Random();
		private Timer timer;

		public UpdateVolumeTimedService(ILogger<UpdateVolumeTimedService> logger, IServiceScopeFactory serviceScopeFactory)
		{
			this.logger = logger;
			this.serviceScopeFactory = serviceScopeFactory;
		}

		public Task StartAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("Timed Hosted Service running.");

			timer = new Timer(DoWork, serviceScopeFactory, TimeSpan.Zero,
				TimeSpan.FromSeconds(delayInSeconds));

			return Task.CompletedTask;
		}

		private int RandomChangeVolume(int currentVolume)
		{
			int diff = (int)Math.Round((-0.1 + rnd.NextDouble() * 0.2) * currentVolume);
			if (currentVolume + diff < 0)
				return 0;
			return currentVolume + diff + 100;
		}

		private async void DoWork(object state)
		{
			using var scope = ((IServiceScopeFactory)state).CreateScope();
			var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			foreach (var tank in await appDbContext.Set<Tank>().ToArrayAsync())
			{
				int changedVolume = RandomChangeVolume(tank.Volume);
				if (changedVolume > tank.MaxVolume)
				{
					logger.LogError($"[id: {tank.Id}] tank's volume exceeds maxVolume.");
				}
				appDbContext.Entry(tank).CurrentValues.SetValues(new { Volume = changedVolume });
			}
			await appDbContext.SaveChangesAsync();
			logger.LogInformation(
				"Timed Hosted Service is working.");
		}

		public Task StopAsync(CancellationToken stoppingToken)
		{
			logger.LogInformation("Timed Hosted Service is stopping.");

			timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			timer?.Dispose();
		}
	}
}
