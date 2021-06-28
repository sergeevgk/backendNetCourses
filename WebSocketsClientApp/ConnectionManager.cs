using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsClientApp
{
	public class ConnectionManager
	{
		public ConnectionManager()
		{
			hubConnections = new List<HubConnection>();
		}

		public List<HubConnection> hubConnections { get; set; }
		
		public async Task ReleaseAllConnections()
		{
			foreach (var connection in hubConnections)
			{
				await connection.StopAsync();
				await connection.DisposeAsync();
			}
			hubConnections.Clear();
			Console.WriteLine("All connections stopped.");
		}
	}
}
