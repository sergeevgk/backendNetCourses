using DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6Wpf.Services
{
	public interface IDataService
	{
		public Task<IReadOnlyCollection<UnitEventDto>> GetUnitEvents(int skip, int take);

		public Task UpdateEvent(UnitEventDto unitEvent);
		public Task DeleteEvent(UnitEventDto unitEvent);
	}
}
