using DAL.Database;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.JsonRepository
{
	public class FactoryJsonRepository : IRepository<Factory>
	{
		private readonly IDatabase db;

		public FactoryJsonRepository(IDatabase db)
		{
			this.db = db;
		}

		public IList<Factory> GetEntities()
		{
			return db.Factories;
		}

		public Factory GetEntityById(int id)
		{
			var result = db.Factories.FirstOrDefault(f => f.Id == id);
			if (result == default(Factory))
			{
				throw new Exception("Factory with such id doesn't exist.");
			}
			return result;
		}

		public Factory CreateEntity(Factory entity)
		{
			var existFactory = db.Factories.FirstOrDefault(f => f.Id == entity.Id);
			if (existFactory == default(Factory))
			{
				db.Factories.Add(entity);
				return entity;
			}
			else
			{
				throw new Exception($"Factory with id {entity.Id} already exists.");
			}
		}

		public Factory UpdateEntity(Factory newEntity)
		{
			var existFactory = db.Factories.FirstOrDefault(f => f.Id == newEntity.Id);
			if (existFactory == default(Factory))
			{
				throw new Exception($"Factory with id {newEntity.Id} doesn't exist.");
			}
			else
			{
				db.Factories.Remove(existFactory);
				db.Factories.Add(newEntity);
				return newEntity;
			}
		}

		public void DeleteEntity(int id)
		{
			var existFactory = db.Factories.FirstOrDefault(f => f.Id == id);
			if (existFactory == default(Factory))
			{
				throw new Exception($"Factory with id {id} doesn't exist.");
			}
			else
			{
				foreach (var unit in db.Units)
				{
					if (unit.FactoryId == id)
						unit.FactoryId = -1;
				}
				db.Factories.Remove(existFactory);
			}
		}
	}
}
