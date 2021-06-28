using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.DatabaseContext;
using WebSocketsDataService.Models;

namespace WebSocketsDataService.Repository
{
	public class UserRolesRepository : IUserRolesRepository
	{
		private readonly ApplicationDbContext dbContext;

		public UserRolesRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		public async Task<IList<UserRole>> GetRoles()
		{
			return await dbContext.Set<UserRole>().ToArrayAsync();
		}
	}
}
