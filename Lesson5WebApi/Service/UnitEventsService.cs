using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using DAL.Models;

namespace Lesson5WebApi.Service
{
	public class UnitEventsService : IUnitEventsService
	{
		private readonly string fileName;
		private readonly int maxResponceCapacity = 100;
		private IEnumerable<UnitEvent> events;
		public UnitEventsService(IEnumerable<UnitEvent> events)
		{
			this.events = events;
		}

		public async Task<IEnumerable<int>> GetActiveEventsByUnitId(int unitId, int take, int skip)
		{
			if (take > maxResponceCapacity)
			{
				take = maxResponceCapacity;
			}
			var eventIds = events
				.Where(e => e.UnitId == unitId)
				.Skip(skip)
				.Take(take)
				.Select(e => e.Id);
			return eventIds;
		}

		public async Task<IEnumerable<UnitEvent>> GetUnitEventsByIds(IEnumerable<int> eventIds)
		{
			return events.Where(e => eventIds.Contains(e.Id));
		}
	}
}
