using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.HubData
{
	public class LinePageChart
	{
		public int Fact { get; set; }
		public int PlanPredict { get; set; }
		public int Predict { get; set; }
		public double PercentageInfluence { get; set; }
		public int DeviationPlanPredictFact { get; set; }
		public int DeviationPlanPredict { get; set; }
		public int Plan { get; set; }
	}

}
