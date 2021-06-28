using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class UnitEventUpdate
	{
		public int Id { get; set; }
		public DateTime Timestamp { get; set; }
		public int UpdatedBy { get; set; }
		public string FieldName { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public int UpdatedEventId { get; set; }
	}

}
