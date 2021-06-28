using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Hubs
{
	public interface IHubSendMessage
	{
		public Task SendMessage();
		public Task SendMessageWithData(object data);
	}
}
