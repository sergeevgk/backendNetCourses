using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.HubData
{
	public class GraphChart
	{
		public FloatGraphPoint[] GraphFact { get; set; }
		public IntGraphPoint[] GraphPlan { get; set; }
	}
	public class FloatGraphPoint
	{
		public float Value { get; set; }
		public DateTime TimeStamp { get; set; }
	}
	public class IntGraphPoint
	{
		public int Value { get; set; }
		public DateTime TimeStamp { get; set; }
	}

}
