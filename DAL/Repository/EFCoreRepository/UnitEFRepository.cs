using DAL.DbContexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EFCoreRepository
{
	public class UnitEFRepository : EFCoreRepository<Unit>, IUnitRepository
	{
		public UnitEFRepository(ApplicationDbContext dbContext) : base(dbContext)
		{

		}

		public async Task<Unit> GetUnitWithTanks(int unitId)
		{
			var unit = dbContext.Unit.Find(unitId);
			await dbContext.Entry(unit).Collection(u => u.Tanks).LoadAsync();
			return unit;
		}
	}
}
