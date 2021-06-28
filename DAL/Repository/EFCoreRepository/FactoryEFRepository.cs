using DAL.DbContexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EFCoreRepository
{
	public class FactoryEFRepository : EFCoreRepository<Factory>, IFactoryRepository
	{
		public FactoryEFRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
		public async Task<Factory> GetFactoryWithUnits(int factoryId)
		{
			var factory = dbContext.Factory.Find(factoryId);
			await dbContext.Entry(factory).Collection(f => f.Units).LoadAsync();
			foreach (var unit in factory.Units)
				await dbContext.Entry(unit).Collection(u => u.Tanks).LoadAsync();
			return factory;
		}
	}
}
