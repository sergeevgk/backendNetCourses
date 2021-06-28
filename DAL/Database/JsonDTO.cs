using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Database
{
	class JsonDTO
	{
		public IList<Factory> Factories { get; set; }
		public IList<Unit> Units { get; set; }
		public IList<Tank> Tanks { get; set; }
	}
}
