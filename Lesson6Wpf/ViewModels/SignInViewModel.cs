using Lesson6Wpf.Helpers;
using Lesson6Wpf.ModelViews;
using Lesson6Wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lesson6Wpf.ViewModels
{
	public class SignInViewModel
	{
		private readonly IAuthenticationService authService;

		public SignInViewModel(IAuthenticationService authService)
		{
			this.authService = authService;
		}

		public string Login { get; set; }
		public string Password { get; set; }
		public ICommand SubmitAuthCommand => new RelayCommand(async o => { await AuthenticateCommandExecuted(); });


		private async Task AuthenticateCommandExecuted()
		{
			await authService.GetAuthenticationToken(Login, Password);
		}
	}
}
