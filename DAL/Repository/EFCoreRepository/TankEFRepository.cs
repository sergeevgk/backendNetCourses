using DAL.DbContexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository.EFCoreRepository
{
	public class TankEFRepository : EFCoreRepository<Tank>, ITankRepository
	{
		public TankEFRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
	}
}
