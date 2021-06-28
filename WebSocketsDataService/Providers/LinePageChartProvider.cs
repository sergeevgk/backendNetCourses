using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Models.HubData;

namespace WebSocketsDataService.Providers
{
	public class LinePageChartProvider : IDataProvider
	{
		public Task<object> GetData()
		{
			Random random = new Random();
			var randomLineChart = new LinePageChart
			{
				DeviationPlanPredict = random.Next(),
				DeviationPlanPredictFact = random.Next(),
				Fact = random.Next(),
				PercentageInfluence = random.NextDouble(),
				Plan = random.Next(),
				PlanPredict = random.Next(),
				Predict = random.Next()
			};
			return Task.FromResult(randomLineChart as object);
		}
	}
}
