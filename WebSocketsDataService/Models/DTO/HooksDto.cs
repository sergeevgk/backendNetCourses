using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.DTO
{
	public class HooksDto
	{
		public IList<string> HubsNames { get; set; }
		public object Data { get; set; }
	}
}
