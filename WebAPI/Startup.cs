
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DAL.Repository.EFCoreRepository;
using DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using DAL.Repository;
using WebAPI.Validation;
using WebAPI.HostedServices;
using Microsoft.AspNetCore.Identity;
using DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using WebAPI.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebAPI.Quartz;
using Quartz.Spi;

namespace WebAPI
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
			// add authorization to all controller methods by default ([Authorize])
			services.AddControllers(o =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();

				o.Filters.Add(new AuthorizeFilter(policy));
			});

			services.AddDbContext<ApplicationDbContext>(options => options
			   .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
					builder => builder.MigrationsAssembly(nameof(DAL))));

			// configure asp net identity: add users and roles definition
			// add DbContext class to create default tables
			// add tokens for 2f-auth, email, pass reset
			services.AddIdentity<User, IdentityRole>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // remove default claims
			
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

			services.AddTransient<IFactoryRepository, FactoryEFRepository>();
			services.AddTransient<IUnitRepository, UnitEFRepository>();
			services.AddTransient<ITankRepository, TankEFRepository>();
			services.AddTransient<IUserRepository, UserEFRepository>();
			services.AddTransient<IUnitEventRepository, UnitEventEFRepository>();
			services.AddTransient<ITokenGenerator, TokenGenerator>();
			services.AddTransient<IJobFactory, JobFactory>();
			services.AddScoped<UnitEventsJob>();

			services.AddSingleton<FactoryValidator>();
			services.AddSingleton<UnitValidator>();
			services.AddSingleton<TankValidator>();
			services.AddSingleton<UnitEventValidator>();

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
			services.AddAutoMapper(typeof(Startup));

			services.AddHostedService<UpdateVolumeTimedService>();
			services.AddHostedService<QuartzHostedService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
