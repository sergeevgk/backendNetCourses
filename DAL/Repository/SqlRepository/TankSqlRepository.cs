using DAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository.SqlRepository
{
	public class TankSqlRepository : IRepository<Tank>
	{
		private readonly string connectionString;

		public TankSqlRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public IList<Tank> GetEntities()
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				return db.Query<Tank>("SELECT * FROM Tank").ToList();
			}
		}

		public Tank GetEntityById(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var result = db.Query<Tank>("SELECT * FROM Tank WHERE Id = @id", new { id }).FirstOrDefault();
				if (result == default(Tank))
				{
					throw new Exception($"Tank with id {id} doesn't exist.");
				}
				return result;
			}
		}

		public Tank CreateEntity(Tank entity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var sqlQuery = "INSERT INTO Tank (Name, Volume, MaxVolume, UnitId) " +
					"VALUES(@Name, @Volume, @MaxVolume, @UnitId); " +
					"SELECT CAST(SCOPE_IDENTITY() as int)";
				int? entityId = db.Query<int>(sqlQuery, entity).FirstOrDefault();
				entity.Id = entityId.Value;
				return entity;
			}
		}

		public Tank UpdateEntity(Tank newEntity)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existTank = GetEntityById(newEntity.Id);
				if (existTank == default(Tank))
				{
					throw new Exception($"Tank with id {newEntity.Id} doesn't exist.");
				}
				else
				{
					var sqlQuery = "UPDATE Tank SET Name = @Name, " +
						"Volume = @Volume, " +
						"MaxVolume = @MaxVolume, " +
						"UnitId = @UnitId, " +
						"WHERE Id = @Id";
					db.Execute(sqlQuery, newEntity);
					return newEntity;
				}
			}
		}

		public void DeleteEntity(int id)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				var existTank = GetEntityById(id);
				if (existTank == default(Tank))
				{
					throw new Exception($"Tank with id {id} doesn't exist.");
				}
				var sqlQuery = "DELETE FROM Tank WHERE Id = @Id";
				db.Execute(sqlQuery, new { id });
			}
		}
	}
}
