using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketsClientApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(Double.Parse(Configuration["Session:Timeout"]));
				options.Cookie.Name = "MySessionCookie";
			});
			services.AddDistributedMemoryCache();

			services.AddRazorPages();
			services.AddAuthentication(auth =>
			{
				auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(token =>
			{
				token.RequireHttpsMetadata = false;
				token.SaveToken = true;
				token.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuthOptions:SecretKey"])),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			services.AddSingleton<ConnectionManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseSession();

			//Add JWTToken to all incoming HTTP Request Header
			app.Use(async (context, next) =>
			{
				var JWTToken = context.Session.GetString("JWTToken");
				if (!string.IsNullOrEmpty(JWTToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + JWTToken);
				}
				await next();
			});
			//Add JWTToken Authentication service
			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
