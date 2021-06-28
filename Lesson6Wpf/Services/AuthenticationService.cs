using Lesson6Wpf.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson6Wpf.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly string loginUri = "https://localhost:44307/api/User/auth";
		private readonly MainWindowViewModel mainViewModel;

		public AuthenticationService(MainWindowViewModel mainViewModel)
		{
			this.mainViewModel = mainViewModel;
		}

		public string AuthenticationToken { get; set; } = "";
		public async Task GetAuthenticationToken(string login, string password)
		{
			try
			{
				using HttpClient client = new HttpClient();
				var loginDataJson = JsonConvert.SerializeObject(new
				{
					login,
					password
				});
				var content = new StringContent(loginDataJson, Encoding.UTF8, "application/json");
				var responce = await client.PostAsync(loginUri, content);
				if (!responce.IsSuccessStatusCode)
				{
					if (responce.StatusCode == HttpStatusCode.BadRequest)
					{
						MessageBox.Show("Invalid login or password. Please, try again");
					}
					else
					{
						MessageBox.Show($"Http GET responded with status code {responce.StatusCode}. " + responce.ReasonPhrase);
					}
					return;
				}
				AuthenticationToken = await responce.Content.ReadAsStringAsync();
				mainViewModel.NavigateToDashboard();
			}
			catch (Exception e)
			{
				MessageBox.Show($"Server or connection error. " + e.Message);
			}
			return;
		}
	}
}
