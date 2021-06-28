using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public interface IUnitRepository : IRepository<Unit>
	{
		public Task<Unit> GetUnitWithTanks(int unitId);
	}
}
