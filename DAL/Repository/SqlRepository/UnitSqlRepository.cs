using DAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository.SqlRepository
{
	public class UnitSqlRepository : IRepository<Unit>
	{
		private readonly string connectionString;

		public UnitSqlRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public IList<Unit> GetEntities()
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				return db.Query<Unit>("SELECT * FROM Unit").ToList();
			}
		}

		public Unit GetEntityById(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var result = db.Query<Unit>("SELECT * FROM Unit WHERE Id = @id", new { id }).FirstOrDefault();
				if (result == default(Unit))
				{
					throw new Exception($"Unit with id {id} doesn't exist.");
				}
				return result;
			}
		}

		public Unit CreateEntity(Unit entity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var sqlQuery = "INSERT INTO Unit (Name, FactoryId) VALUES(@Name, @FactoryId); SELECT CAST(SCOPE_IDENTITY() as int)";
				int? entityId = db.Query<int>(sqlQuery, entity).FirstOrDefault();
				entity.Id = entityId.Value;
				return entity;
			}
		}

		public Unit UpdateEntity(Unit newEntity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existUnit = GetEntityById(newEntity.Id);
				if (existUnit == default(Unit))
				{
					throw new Exception($"Unit with id {newEntity.Id} doesn't exist.");
				}
				else
				{
					var sqlQuery = "UPDATE Unit SET Name = @Name, FactoryId = @FactoryId WHERE Id = @Id";
					db.Execute(sqlQuery, newEntity);
					return newEntity;
				}
			}
		}
		public void DeleteEntity(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existUnit = GetEntityById(id);
				if (existUnit == default(Unit))
				{
					throw new Exception($"Unit with id {id} doesn't exist.");
				}

				var sqlQuery = "DELETE FROM Unit WHERE Id = @Id";
				db.Execute(sqlQuery, new { id });
			}
		}
	}
}
