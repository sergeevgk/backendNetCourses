using DAL.DbContexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EFCoreRepository
{
	public class UnitEventEFRepository : EFCoreRepository<UnitEvent>, IUnitEventRepository
	{
		public UnitEventEFRepository(ApplicationDbContext dbContext) : base(dbContext)
		{

		}

		public IEnumerable<UnitEvent> GetUnitEvents(int take, int skip)
		{
			return dbContext.Set<UnitEvent>().Skip(skip).Take(take).AsEnumerable();
		}
	}
}
