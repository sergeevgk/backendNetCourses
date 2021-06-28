using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public interface IFactoryRepository : IRepository<Factory>
	{
		public Task<Factory> GetFactoryWithUnits(int factoryId);
	}
}
