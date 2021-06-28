using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebSocketsClientApp.Models;

namespace WebSocketsClientApp.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IConfiguration configuration;

		public IndexModel(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[BindProperty]
		public LoginViewModel LoginModel { get; set; }

		public IActionResult OnGet()
		{
			return this.Page();
		}

		public async Task<IActionResult> OnPostLogIn()
		{
			try
			{
				if (ModelState.IsValid)
				{
					var httpClient = new HttpClient();
					var authUrl = configuration["ServiceUrls:AuthenticationService"];

					var loginDataJson = JsonConvert.SerializeObject(new { LoginModel.Login, LoginModel.Password });
					var content = new StringContent(loginDataJson, Encoding.UTF8, "application/json");
					var responce = await httpClient.PostAsync(authUrl, content);

					if (!responce.IsSuccessStatusCode)
					{
						if (responce.StatusCode == HttpStatusCode.BadRequest)
						{
							return BadRequest("Login-password pair is not valid");
						}
						else
						{
							return Unauthorized();
						}
					}
					var authenticationToken = await responce.Content.ReadAsStringAsync();
					if (authenticationToken != null)
					{
						HttpContext.Session.SetString("JWTToken", authenticationToken);
					}
					return this.RedirectToPage("UserPages/Index");
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex);
			}
			return this.Page();
		}
	}
}
