using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContexts
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public DbSet<Factory> Factory { get; set; }
		public DbSet<Unit> Unit { get; set; }
		public DbSet<Tank> Tank { get; set; }
		public DbSet<UnitEvent> UnitEvent { get; set; }
		public DbSet<UnitEventUpdate> UnitEventUpdateLogs { get; set; }

		public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
		}
		// вместо хеширования прописать строку с паролем (хэш)
		// или вынести в startup
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var admin = new User
			{
				Id = "1",
				Email = "admin123@mail.ru",
				UserName = "admin"
			};
			admin.PasswordHash = "AQAAAAEAACcQAAAAEPirGBw/4nqnALj1mZU/9A/RGCfIL22PS/SO/UjUERmia7P1yOmV5Lob9OklpFHlRw==";
			modelBuilder.Entity<User>().HasData(admin);
			var serviceAccount = new User
			{
				Id = "2",
				Email = "service@service.ru",
				UserName = "service",
				PasswordHash = "AQAAAAEAACcQAAAAEPirGBw/4nqnALj1mZU/9A/RGCfIL22PS/SO/UjUERmia7P1yOmV5Lob9OklpFHlRw=="
			};
			modelBuilder.Entity<User>().HasData(serviceAccount);
			var roles = new IdentityRole[]
			{
				new IdentityRole{ Id = "1", Name = "Administrator", NormalizedName = "Administrator".ToUpper()},
				new IdentityRole{ Id = "2", Name = "Manager", NormalizedName = "Manager".ToUpper()},
				new IdentityRole{ Id = "3", Name = "User", NormalizedName = "User".ToUpper()},
				new IdentityRole{ Id = "4", Name = "Service", NormalizedName = "Service".ToUpper()}
			};
			modelBuilder.Entity<IdentityRole>().HasData(roles);

			var userRoleRelations = new IdentityUserRole<string>[] {
				new IdentityUserRole<string>{ UserId = "1", RoleId = "1"},
				new IdentityUserRole<string>{ UserId = "2", RoleId = "4"}
			};
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoleRelations);
			base.OnModelCreating(modelBuilder);
		}
	}
}
