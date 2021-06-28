using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public interface IUnitEventRepository : IRepository<UnitEvent>
	{
		public IEnumerable<UnitEvent> GetUnitEvents(int take, int skip);
	}
}
