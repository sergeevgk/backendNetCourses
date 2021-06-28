using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketsDataService.Models.HubData;
using WebSocketsDataService.Models.HubData.HubDataDTO;
using WebSocketsDataService.Providers;

namespace WebSocketsDataService.Hubs
{
	public class SimpleChartHub : BaseHub, IHubSendMessage
	{
		public SimpleChartHub(IDataProviderFactory dataProviderFactory, IMapper mapper) : base(dataProviderFactory, typeof(SimpleChart), mapper)
		{
			this.Name = "simpleChart";
		}

		

		public async override Task SendMessage()
		{
			var data = (SimpleChart) await dataProvider.GetData();
			var tasks = new List<Task>();
			if (Clients != null)
			{
				tasks.Add(SendCustomizedData<SimpleChartUserDto>("User", data));
				tasks.Add(SendCustomizedData<SimpleChartManagerDto>("Manager", data));
				tasks.Add(SendCustomizedData<SimpleChartAdminDto>("Administrator", data));
				await Task.WhenAll(tasks);
			}
		}
	}
}
