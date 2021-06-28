using DAL.DbContexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EFCoreRepository
{
	public abstract class EFCoreRepository<TEntity> : IRepository<TEntity> where TEntity: NamedDbObject
	{
		protected readonly ApplicationDbContext dbContext;

		protected EFCoreRepository(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IList<TEntity>> GetEntities()
		{
			return await dbContext.Set<TEntity>().ToArrayAsync();
		}

		public async Task<TEntity> GetEntityById(int id)
		{
			return await dbContext.Set<TEntity>().FindAsync(id);
		}
		public async Task<TEntity> CreateEntity(TEntity entity)
		{
			dbContext.Set<TEntity>().Add(entity);
			await dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<TEntity> UpdateEntity(TEntity newEntity)
		{
			var existEntity = await GetEntityById(newEntity.Id);
			if (existEntity == null)
			{
				throw new Exception($"Entity with id {newEntity.Id} does not exist.");
			}
			dbContext.Entry<TEntity>(existEntity).CurrentValues.SetValues(newEntity);
			await dbContext.SaveChangesAsync();
			return newEntity;
		}
		public async Task DeleteEntity(int id)
		{
			var existEntity = await GetEntityById(id);
			if (existEntity == null)
			{
				throw new Exception($"Entity with id {id} does not exist.");
			}
			dbContext.Set<TEntity>().Remove(existEntity);
			await dbContext.SaveChangesAsync();	
		}
	}
}
