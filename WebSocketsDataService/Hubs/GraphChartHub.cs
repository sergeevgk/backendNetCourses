using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Models.HubData;
using WebSocketsDataService.Models.HubData.HubDataDTO;
using WebSocketsDataService.Providers;

namespace WebSocketsDataService.Hubs
{
	public class GraphChartHub : BaseHub, IHubSendMessage
	{
		public GraphChartHub(IDataProviderFactory dataProviderFactory, IMapper mapper) : base(dataProviderFactory, typeof(GraphChart), mapper)
		{
			this.Name = "graphChart";
		}

		public async override Task SendMessage()
		{
			var data = (GraphChart)await dataProvider.GetData();
			var tasks = new List<Task>();
			if (Clients != null)
			{
				tasks.Add(SendCustomizedData<GraphChartDto>("User", data));
				tasks.Add(SendCustomizedData<GraphChartDto>("Manager", data));
				tasks.Add(SendCustomizedData<GraphChartDto>("Administrator", data));
				await Task.WhenAll(tasks);
			}
		}
	}
}
