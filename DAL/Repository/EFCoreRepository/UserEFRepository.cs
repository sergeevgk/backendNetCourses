using DAL.DbContexts;
using DAL.Models;
using DAL.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EFCoreRepository
{
	public class UserEFRepository : IUserRepository
	{
		private readonly ApplicationDbContext dbContext;

		public UserEFRepository(ApplicationDbContext dbContext)
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
			return await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == login);
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
