using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.HubData
{
	public class SimpleChart
	{
		public string Name { get; set; }
		public int Min { get; set; }
		public int Max { get; set; }
		public int Value { get; set; }
		public int Deviation { get; set; }
	}

}
