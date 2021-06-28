using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5WebApi.Service
{
	public interface IUnitEventsService
	{
		public Task<IEnumerable<int>> GetActiveEventsByUnitId(int unitId, int take, int skip);
		public Task<IEnumerable<UnitEvent>> GetUnitEventsByIds(IEnumerable<int> eventIds);
	}
}
