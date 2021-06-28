using DAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository.SqlRepository
{
	public class FactorySqlRepository : IRepository<Factory>
	{
		private readonly string connectionString;

		public FactorySqlRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public IList<Factory> GetEntities()
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				return db.Query<Factory>("SELECT * FROM Factory").ToList();
			}
		}

		public Factory GetEntityById(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var result = db.Query<Factory>("SELECT * FROM Factory WHERE Id = @id", new { id }).FirstOrDefault();
				if (result == default(Factory))
				{
					throw new Exception($"Factory with id {id} doesn't exist.");
				}
				return result;
			}
		}

		public Factory CreateEntity(Factory entity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var sqlQuery = "INSERT INTO Factory (Name, Description) VALUES(@Name, @Description); SELECT CAST(SCOPE_IDENTITY() as int)";
				int? entityId = db.Query<int>(sqlQuery, entity).FirstOrDefault();
				entity.Id = entityId.Value;
				return entity;
			}
		}

		public Factory UpdateEntity(Factory newEntity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existFactory = GetEntityById(newEntity.Id);
				if (existFactory == default(Factory))
				{
					throw new Exception($"Factory with id {newEntity.Id} doesn't exist.");
				}
				else
				{
					var sqlQuery = "UPDATE Factory SET Name = @Name, Description = @Description WHERE Id = @Id";
					db.Execute(sqlQuery, newEntity);
					return newEntity;
				}
			}
		}

		public void DeleteEntity(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existFactory = GetEntityById(id);
				if (existFactory == default(Factory))
				{
					throw new Exception($"Factory with id {id} doesn't exist.");
				}

				var sqlQuery = "DELETE FROM Factory WHERE Id = @Id";
				db.Execute(sqlQuery, new { id });
			}
		}
	}
}
