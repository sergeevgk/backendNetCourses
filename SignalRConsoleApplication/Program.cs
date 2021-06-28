using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DaDataConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			string hubUrl = "https://localhost:44345/dadata";
			await using HubConnection hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
			await hubConnection.StartAsync().ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					Console.WriteLine($"Error on connecting to hub {hubUrl}. " + task.Exception);
				}
				else
				{
					Console.WriteLine($"Successfully connected to hub {hubUrl}");
					hubConnection.On<string>("FindCompany", s => { Console.WriteLine("Название компании: {0}", s); });
					Console.ReadLine();
				}
			});
			await hubConnection.StopAsync();
		}
	}
}
