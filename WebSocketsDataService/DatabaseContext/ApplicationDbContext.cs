using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSocketsDataService.Models;

namespace WebSocketsDataService.DatabaseContext
{
	public class ApplicationDbContext : DbContext
	{
		private readonly IPasswordHasher<User> passwordHasher;

		public ApplicationDbContext(DbContextOptions options, IPasswordHasher<User> passwordHasher) : base(options)
		{
			this.passwordHasher = passwordHasher;
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserClaims { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var roles = new UserRole[]
			{
				new UserRole{ Id = "1", Name = "Administrator"},
				new UserRole{ Id = "2", Name = "Manager"},
				new UserRole{ Id = "3", Name = "User"},
				new UserRole{ Id = "4", Name = "Service"}
			};
			modelBuilder.Entity<UserRole>().HasData(roles);

			var admin = new User
			{
				Id = "1",
				Login = "admin",
				Name = "admin",
				RoleId = "1"
			};
			admin.HashedPassword = "AQAAAAEAACcQAAAAEPO20MgnC3bgnulxetBENGuxg8hjRXXdJRoB9dEXDoSBM2NA3ek6vImtDKouDdEOAA=="; // "pwd123"
			modelBuilder.Entity<User>().HasData(admin);

			base.OnModelCreating(modelBuilder);
		}
	}
}
