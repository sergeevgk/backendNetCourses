using DaDataConsoleApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaDataWebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CompanyController : ControllerBase
	{
		private readonly IApiService service;
		private readonly IHubContext<CustomHub> hubContext;

		public CompanyController(IApiService service, IHubContext<CustomHub> hubContext)
		{
			this.service = service;
			this.hubContext = hubContext;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<string>> Get(string id)
		{
			var partyName = await service.GetCompanyNameById(id);
			if (partyName == null)
			{
				return NotFound("Company was not found.");
			}
			await hubContext.Clients.All.SendAsync("FindCompany", partyName);
			return Ok(partyName);
		}
	}
}
