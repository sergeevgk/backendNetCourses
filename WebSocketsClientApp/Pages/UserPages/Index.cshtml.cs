using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebSocketsDataService.Hubs;

namespace WebSocketsClientApp.Pages.UserPages
{
	public class IndexModel : PageModel
	{
		private readonly IConfiguration configuration;

		public IndexModel(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string authToken { get; set; }
		public IEnumerable<BaseHub> Channels { get; set; }

		[BindProperty]
		public List<string> Subscriptions { get; set; }
		public async Task OnGet()
		{
			authToken = HttpContext.Session.GetString("JWTToken");
			// http get list
			var httpClient = new HttpClient();
			var channelsUrl = configuration["ServiceUrls:ChannelsService"];
			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", authToken);
			var responce = await httpClient.GetAsync(channelsUrl);

			if (responce.IsSuccessStatusCode)
			{
				var responceText = await responce.Content.ReadAsStringAsync();
				Channels = JsonConvert.DeserializeObject<IEnumerable<BaseHub>>(responceText);
			}
		}

		public async Task<IActionResult> OnPostLogOut()
		{
			HttpContext.Session.Clear();
			return this.RedirectToPage("/Index");
		}	
		public async Task<IActionResult> OnPostConnect()
		{
			HttpContext.Session.SetString("Subscriptions", JsonConvert.SerializeObject(Subscriptions));
			return this.RedirectToPage("ChannelPage");
		}
	}
}
