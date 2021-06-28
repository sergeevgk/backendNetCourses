using DAL.Database;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repository.JsonRepository
{
	public class TankJsonRepository : IRepository<Tank>
	{
		private readonly IDatabase db;

		public TankJsonRepository(IDatabase db)
		{
			this.db = db;
		}

		public IList<Tank> GetEntities()
		{
			return db.Tanks;
		}

		public Tank GetEntityById(int id)
		{
			var result = db.Tanks.FirstOrDefault(t => t.Id == id);
			if (result == default(Tank))
			{
				throw new Exception("Tank with such id doesn't exist.");
			}
			return result;
		}

		public Tank CreateEntity(Tank entity)
		{
			var existTank = db.Tanks.FirstOrDefault(t => t.Id == entity.Id);
			if (existTank == default(Tank))
			{
				db.Tanks.Add(entity);
				return entity;
			}
			else
			{
				throw new Exception($"Tank with id {entity.Id} already exists.");
			}
		}

		public Tank UpdateEntity(Tank newEntity)
		{
			var existTank = db.Tanks.FirstOrDefault(t => t.Id == newEntity.Id);
			if (existTank == default(Tank))
			{
				throw new Exception($"Tank with id {newEntity.Id} doesn't exist.");
			}
			else
			{
				db.Tanks.Remove(existTank);
				db.Tanks.Add(newEntity);
				return newEntity;
			}
		}

		public void DeleteEntity(int id)
		{
			var existTank = db.Tanks.FirstOrDefault(t => t.Id == id);
			if (existTank == default(Tank))
			{
				throw new Exception($"Tank with id {id} doesn't exist.");
			}
			else
			{
				db.Tanks.Remove(existTank);
			}
		}

	}
}
