using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Providers;

namespace WebSocketsDataService.Hubs
{
	[Authorize]
	public class BaseHub : Hub
	{
		protected readonly IDataProvider dataProvider;
		private readonly IMapper mapper;

		public Guid Id { get; set; } = Guid.NewGuid();
		public string Type { get; set; }
		public string Name { get; set; }

		protected BaseHub(IDataProviderFactory dataProviderFactory, Type dataType, IMapper mapper)
		{
			dataProvider = dataProviderFactory.Create(dataType);
			Type = dataType.Name;
			this.mapper = mapper;
		}

		public BaseHub()
		{
		}

		protected async Task SendCustomizedData<TData>(string clientTypeName, object data)
		{
			var customizedData = mapper.Map<TData>(data);
			var textValue = JsonConvert.SerializeObject(customizedData);
			await Clients.All.SendAsync("SendMessage" + clientTypeName, textValue);
		}

		public virtual async Task SendMessage()
		{
			var data = await dataProvider.GetData();
			string textValue = JsonConvert.SerializeObject(data);
			if (Clients != null)
			{
				await Clients.All.SendAsync("SendMessage", textValue);
			}
		}
		public virtual async Task SendMessageWithData(object data)
		{
			string textValue = JsonConvert.SerializeObject(data);
			if (Clients != null)
			{
				await Clients.All.SendAsync("SendMessageUser" , textValue);
				await Clients.All.SendAsync("SendMessageManager", textValue);
				await Clients.All.SendAsync("SendMessageAdministrator", textValue);

			}
		}
	}
}
