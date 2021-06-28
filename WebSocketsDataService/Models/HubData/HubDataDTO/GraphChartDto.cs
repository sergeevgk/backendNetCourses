using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.HubData.HubDataDTO
{
	public class GraphChartDto
	{
		public FloatGraphPoint[] GraphFact { get; set; }
		public IntGraphPoint[] GraphPlan { get; set; }
	}
}
