using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Text;
using System.Net;
using Lesson6Wpf.ViewModels;
using DAL.Models.DTO;

namespace Lesson6Wpf.Services
{
	public class DataService : IDataService
	{
		private readonly string getUnitEventsUri = "https://localhost:44307/api/UnitEvent/all";
		private readonly string unitEventUri = "https://localhost:44307/api/UnitEvent/";
		private readonly IAuthenticationService authService;
		private readonly MainWindowViewModel mainViewModel;

		public DataService(IAuthenticationService authService, MainWindowViewModel mainViewModel)
		{
			this.authService = authService;
			this.mainViewModel = mainViewModel;
		}

		public async Task<IReadOnlyCollection<UnitEventDto>> GetUnitEvents(int skip, int take)
		{
			try
			{
				using HttpClient client = new HttpClient();
				string unitsUri = getUnitEventsUri + $"?take={take}&skip={skip}";
				client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", authService.AuthenticationToken);
				var responce = await client.GetAsync(unitsUri);
				if (!responce.IsSuccessStatusCode)
				{
					if (responce.StatusCode == HttpStatusCode.Unauthorized)
					{
						MessageBox.Show($"Authorization failed, please sign in again. " + responce.ReasonPhrase);
						mainViewModel.NavigateToSignIn();
						return new List<UnitEventDto>();
					}
					MessageBox.Show($"Http GET responded with status code {responce.StatusCode}. " + responce.ReasonPhrase);
					return new List<UnitEventDto>();
				}
				var jsonString = await responce.Content.ReadAsStringAsync();
				var units = JsonConvert.DeserializeObject<IReadOnlyCollection<UnitEventDto>>(jsonString);
				return units;
			}
			catch (Exception e)
			{
				MessageBox.Show($"Server or connection error. " + e.Message);
			}
			return new List<UnitEventDto>();
		}

		public async Task UpdateEvent(UnitEventDto unitEvent)
		{
			try
			{
				using HttpClient client = new HttpClient();
				string unitUpdateUri = unitEventUri + $"{unitEvent.Id}";
				var jsonUnit = JsonConvert.SerializeObject(unitEvent);
				var stringContent = new StringContent(jsonUnit, Encoding.UTF8, "application/json");
				client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", authService.AuthenticationToken);
				var responce = await client.PutAsync(unitUpdateUri, stringContent);
				if (!responce.IsSuccessStatusCode)
				{
					if (responce.StatusCode == HttpStatusCode.Unauthorized)
					{
						MessageBox.Show($"Authorization failed, please sign in again. " + responce.ReasonPhrase);
						mainViewModel.NavigateToSignIn();
					}
					return;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show($"Server or connection error. " + e.Message);
			}
		}
		public async Task DeleteEvent(UnitEventDto unitEvent)
		{
			try
			{
				using HttpClient client = new HttpClient();
				string unitUpdateUri = unitEventUri + $"{unitEvent.Id}";
				client.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", authService.AuthenticationToken);
				var responce = await client.DeleteAsync(unitUpdateUri);
				if (!responce.IsSuccessStatusCode)
				{
					if (responce.StatusCode == HttpStatusCode.Unauthorized)
					{
						MessageBox.Show($"Authorization failed, please sign in again. " + responce.ReasonPhrase);
						mainViewModel.NavigateToSignIn();
					}
					return;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show($"Server or connection error. " + e.Message);
			}
		}
	}
}