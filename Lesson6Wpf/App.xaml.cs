using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lesson6Wpf.ModelViews;
using Lesson6Wpf.Services;
using Lesson6Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lesson6Wpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly IHost host;
		public App()
		{
			host = new HostBuilder()
		   .ConfigureServices((hostContext, services) =>
		   {
			   services.AddSingleton<IDataService, DataService>();
			   services.AddSingleton<IAuthenticationService, AuthenticationService>();
			   services.AddSingleton<MainWindowViewModel>();

		   }).Build();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			var mainWindowViewModel = host.Services.GetService<MainWindowViewModel>();
			var mainWindow = new MainWindow { DataContext = mainWindowViewModel };

			var dashBoardViewModel = new DashboardViewModel(host.Services.GetService<IDataService>());
			mainWindowViewModel.DashboardView = new DashboardView { DataContext = dashBoardViewModel };
			mainWindowViewModel.DashboardViewModel = dashBoardViewModel;

			var loginViewModel = new SignInViewModel(host.Services.GetService<IAuthenticationService>());
			mainWindowViewModel.SignInView = new SignInView { DataContext = loginViewModel };
			mainWindowViewModel.SignInViewModel = loginViewModel;

			mainWindowViewModel.InitMainWindow(mainWindow);
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			using (host)
			{
				await host.StopAsync();
			}

			base.OnExit(e);
		}
	}
}
