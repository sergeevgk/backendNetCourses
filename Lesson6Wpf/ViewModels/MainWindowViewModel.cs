using System.Windows;
using Lesson6Wpf.ModelViews;

namespace Lesson6Wpf.ViewModels
{
	public class MainWindowViewModel
	{
		public MainWindow MainWindow { get; set; }

		public DashboardView DashboardView { get; set; }
		public DashboardViewModel DashboardViewModel { get; set; }
		public SignInView SignInView { get; set; }
		public SignInViewModel SignInViewModel { get; set; }

		public MainWindowViewModel()
		{
		}

		public void InitMainWindow(MainWindow mainWindow)
		{
			MainWindow = mainWindow;
			MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			MainWindow.Show();
			NavigateToSignIn();
		}

		public void NavigateToDashboard()
		{
			MainWindow.FrameBody.NavigationService.Navigate(DashboardView);
		}
		public void NavigateToSignIn()
		{
			MainWindow.FrameBody.NavigationService.Navigate(SignInView);
		}

	}
}