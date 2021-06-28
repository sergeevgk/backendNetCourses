using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz.Spi;
using System;
using System.Text;
using WebSocketsDataService.Authorization;
using WebSocketsDataService.DatabaseContext;
using WebSocketsDataService.HostedServices;
using WebSocketsDataService.Hubs;
using WebSocketsDataService.Models;
using WebSocketsDataService.Providers;
using WebSocketsDataService.Quartz;
using WebSocketsDataService.Repository;

namespace WebSocketsDataService
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
			services.AddControllers(o =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();

				o.Filters.Add(new AuthorizeFilter(policy));
			});

			services
				.AddAuthorization(options =>
				{
					options.AddPolicy("RequireAdministratorRole",
						policy => policy.RequireRole("Administrator"));
					options.AddPolicy("RequireServiceRole",
						policy => policy.RequireRole("Service"));
					options.AddPolicy("RequireManagerOrAdmin",
						policy => policy.RequireRole("Administrator", "Manager"));
					options.AddPolicy("ElevatedRights", policy =>
						policy.RequireRole("Administrator", "Manager", "User"));
				});


			services.AddDbContext<ApplicationDbContext>(options => options
			   .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddControllers();

			services.AddSingleton<ITokenGenerator, TokenGenerator>();
			services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

			services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
			services.AddSingleton<SimpleChartHub>();
			services.AddSingleton<GraphChartHub>();

			services.AddTransient<IJobFactory, JobFactory>();

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserRolesRepository, UserRolesRepository>();

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

				})
				.AddJwtBearer(cfg =>
				{
					cfg.RequireHttpsMetadata = false;
					cfg.SaveToken = true;
					cfg.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						RequireExpirationTime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuthOptions:SecretKey"])),
						ValidateIssuerSigningKey = true,
						ClockSkew = TimeSpan.Zero
					};
				});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description =
					"JWT Authorization header using the Bearer scheme." + Environment.NewLine +
					"Enter your token in the text input below." + Environment.NewLine +
					"Example: \"12345abcdef\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer"
				});
				c.OperationFilter<ApplyOAuth2Security>();
			});

			services.AddCors(options => options.AddDefaultPolicy(builder => builder.SetIsOriginAllowedToAllowWildcardSubdomains()
				.WithOrigins(Configuration["Cors:Origin"])
				.AllowAnyMethod()
				.AllowCredentials()
				.AllowAnyHeader()));

			services.AddAutoMapper(typeof(Startup));
			services.AddHostedService<QuartzHostedService>();
			services.AddSignalR();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebSocketsDataService v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<SimpleChartHub>("/simpleChart");
				endpoints.MapHub<GraphChartHub>("/graphChart");
			});
		}
	}
}
