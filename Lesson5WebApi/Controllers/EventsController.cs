using DAL.Models;
using Lesson5WebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson5WebApi.Controllers
{
	[Authorize(Policy = "RequireServiceRole")]
	[ApiController]
	[Route("api/[controller]")]
	public class EventsController : ControllerBase
	{
		private readonly ILogger<EventsController> logger;
		private readonly IUnitEventsService service;

		public EventsController(ILogger<EventsController> logger, IUnitEventsService service)
		{
			this.logger = logger;
			this.service = service;
		}

		[Route("keys")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<int>>> Get(int unitId, int take, int skip)
		{
			try
			{
				var ids = await service.GetActiveEventsByUnitId(unitId, take, skip);
				return Ok(ids);
			}
			catch (Exception ex)
			{
				logger.LogError("Error on reading active event ids. " + ex.Message);
				throw;
			}
		}

		[HttpPost]
		public async Task<ActionResult<IEnumerable<UnitEvent>>> Post([FromBody] IEnumerable<int> eventIds)
		{
			try
			{
				var events = await service.GetUnitEventsByIds(eventIds);
				return Ok(events);
			}
			catch (Exception ex)
			{
				logger.LogError("Error on reading active event. " + ex.Message);
				throw;
			}
		}
	}
}
