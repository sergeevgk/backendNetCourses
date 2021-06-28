using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebSocketsDataService.Hubs;
using WebSocketsDataService.Models.DTO;

namespace WebSocketsDataService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChannelController : ControllerBase
	{

		private readonly ILogger<ChannelController> logger;
		private readonly IServiceProvider serviceProvider;
		private readonly IMapper mapper;

		public ChannelController(ILogger<ChannelController> logger,
								IServiceProvider serviceProvider,
								IMapper mapper)
		{
			this.logger = logger;
			this.serviceProvider = serviceProvider;
			this.mapper = mapper;
		}

		[Authorize(Policy = "ElevatedRights")]
		[HttpGet("all")]
		public ActionResult<IEnumerable<BaseHubDto>> Get()
		{
			var channels = new List<BaseHub>();
			var hubTypes = Assembly.GetAssembly(GetType())
				.GetTypes()
				.Where(t => !t.IsAbstract)
				.Where(t => t.IsAssignableTo(typeof(BaseHub)));

			foreach (var hubType in hubTypes)
			{
				if (serviceProvider.GetService(hubType) is BaseHub channel)
				{
					channels.Add(channel);
				}
			}

			return Ok(mapper.Map<IEnumerable<BaseHubDto>>(channels));
		}
	}
}
