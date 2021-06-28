using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Models.HubData;

namespace WebSocketsDataService.Providers
{
	public class SimpleChartProvider : IDataProvider
	{
		public Task<object> GetData()
		{
			Random random = new Random();
			int min = random.Next(Int32.MinValue, 0);
			int max = min + random.Next(1, Int32.MaxValue);
			var simpleChart = new SimpleChart
			{
				Name = "chart " + random.Next().ToString(),
				Value = random.Next(min, max),
				Deviation = random.Next(),
				Min = min,
				Max = max
			};
			return Task.FromResult(simpleChart as object);
		}
	}
}
