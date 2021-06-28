using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
	public interface IDatabase
	{
		public IList<Factory> Factories { get; set; }
		public IList<Unit> Units { get; set; }
		public IList<Tank> Tanks { get; set; }
		public Task SaveAsync();

		public Task ReadAsync();

		public Task SeedData();
	}
}
