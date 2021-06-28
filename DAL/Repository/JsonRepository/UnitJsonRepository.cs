using DAL.Database;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repository.JsonRepository
{
	public class UnitJsonRepository : IRepository<Unit>
	{
		private readonly IDatabase db;

		public UnitJsonRepository(IDatabase db)
		{
			this.db = db;
		}


		public IList<Unit> GetEntities()
		{
			return db.Units;
		}

		public Unit GetEntityById(int id)
		{
			var result = db.Units.FirstOrDefault(u => u.Id == id);
			if (result == default(Unit))
			{
				throw new Exception("Unit with such id doesn't exist.");
			}
			return result;
		}

		public Unit CreateEntity(Unit entity)
		{
			var existUnit = db.Units.FirstOrDefault(u => u.Id == entity.Id);
			if (existUnit == default(Unit))
			{
				db.Units.Add(entity);
				return entity;
			}
			else
			{
				throw new Exception($"Unit with id {entity.Id} already exists.");
			}
		}

		public Unit UpdateEntity(Unit newEntity)
		{
			var existUnit = db.Units.FirstOrDefault(u => u.Id == newEntity.Id);
			if (existUnit == default(Unit))
			{
				throw new Exception($"Unit with id {newEntity.Id} doesn't exist.");
			}
			else
			{
				db.Units.Remove(existUnit);
				db.Units.Add(newEntity);
				return newEntity;
			}
		}
		public void DeleteEntity(int id)
		{
			var existUnit = db.Units.FirstOrDefault(u => u.Id == id);
			if (existUnit == default(Unit))
			{
				throw new Exception($"Unit with id {id} doesn't exist.");
			}
			else
			{
				foreach (var tank in db.Tanks)
				{
					if (tank.UnitId == id)
						tank.UnitId = -1;
				}
				db.Units.Remove(existUnit);
			}
		}

	}
}
