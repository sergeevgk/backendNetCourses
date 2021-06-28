using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebSocketsDataService.Hubs;
using WebSocketsDataService.Models.DTO;

namespace WebSocketsDataService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HooksController : ControllerBase
	{
		private readonly IServiceProvider serviceProvider;
		private readonly IEnumerable<IHubSendMessage> hubActions;

		public HooksController(IServiceProvider serviceProvider, IEnumerable<IHubSendMessage> hubActions)
		{
			this.serviceProvider = serviceProvider;
			this.hubActions = hubActions;
		}

		[Authorize]
		[HttpGet]
		public ActionResult<IEnumerable<string>> GetHubsNames()
		{
			var hubNames = Assembly.GetAssembly(GetType())
				.GetTypes()
				.Where(t => !t.IsAbstract)
				.Where(t => t.IsAssignableTo(typeof(BaseHub)))
				.Where(t => serviceProvider.GetService(t) is BaseHub)
				.Select(channel => channel.Name);

			return Ok(hubNames);
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> SendMessage([FromBody]HooksDto model)
		{
			List<Task> tasks = new List<Task>();
			if (model.HubsNames == null || model.HubsNames.Count == 0)
				return Ok();
			var filteredHubs = hubActions.Where(h => model.HubsNames.Contains(h.GetType().Name));
			foreach (var hub in filteredHubs)
			{
				tasks.Add(hub.SendMessageWithData(model.Data));
			}
			await Task.WhenAll(tasks);
			return Ok();
		}

		[Authorize]
		[HttpPost("all")]
		public async Task<ActionResult> SendMessageAll([FromBody] object data)
		{
			List<Task> tasks = new List<Task>();

			foreach (var hub in hubActions)
			{
				tasks.Add(hub.SendMessageWithData(data));
			}
			await Task.WhenAll(tasks);
			return Ok();
		}
	}
}
