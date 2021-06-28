using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.DatabaseContext;
using WebSocketsDataService.Models;

namespace WebSocketsDataService.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext dbContext;

		public UserRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IList<User>> GetUsers()
		{
			return await dbContext.Set<User>().ToArrayAsync();
		}

		public async Task<User> GetUserById(string id)
		{
			return await dbContext.Set<User>().FindAsync(id);
		}

		public async Task<User> GetUserByLogin(string login)
		{
			return await dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == login);
		}

		public async Task<User> CreateUser(User entity)
		{
			dbContext.Set<User>().Add(entity);
			await dbContext.SaveChangesAsync();
			return entity;
		}
		public async Task<User> UpdateUser(User newEntity)
		{
			var existEntity = await GetUserById(newEntity.Id);
			if (existEntity == null)
			{
				throw new Exception($"User with id {newEntity.Id} does not exist.");
			}
			dbContext.Entry<User>(existEntity).CurrentValues.SetValues(newEntity);
			await dbContext.SaveChangesAsync();
			return newEntity;
		}

		public async Task DeleteUser(string id)
		{
			var existEntity = await GetUserById(id);
			if (existEntity == null)
			{
				throw new Exception($"User with id {id} does not exist.");
			}
			dbContext.Set<User>().Remove(existEntity);
			await dbContext.SaveChangesAsync();
		}
	}
}
