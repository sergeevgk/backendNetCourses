using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTO
{
	public class UnitEventDto
	{
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public double StorageValue { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Tags { get; set; }
		public string ResponsibleOperators { get; set; }
	}
}
