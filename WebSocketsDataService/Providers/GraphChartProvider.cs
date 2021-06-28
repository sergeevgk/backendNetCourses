using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Models.HubData;

namespace WebSocketsDataService.Providers
{
	public class GraphChartProvider : IDataProvider
	{
		public Task<object> GetData()
		{
			Random random = new Random();
			var randomGraph = new GraphChart
			{
				GraphFact = Enumerable
						.Range(1, random.Next(1, 4))
						.Select(index => new FloatGraphPoint
						{
							TimeStamp = DateTime.Now.AddMinutes(random.NextDouble()),
							Value = random.Next()
						})
						.ToArray(),
				GraphPlan = Enumerable
						.Range(1, random.Next(1,4))
						.Select(index => new IntGraphPoint
						{
							TimeStamp = DateTime.Now.AddMinutes(random.NextDouble()),
							Value = random.Next()
						})
						.ToArray(),
			};
			return Task.FromResult(randomGraph as object);
		}
	}
}
