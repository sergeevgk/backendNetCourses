using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DTO
{
	public class FactoryWithUnitsDto
	{    
        public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IList<Unit> Units { get; set; }
	}
}
