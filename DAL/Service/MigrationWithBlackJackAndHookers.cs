using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.Service
{
	public class MigrationWithBlackJackAndHookers : IMigrationWithBlackJackAndHookers
	{
		private readonly string connectionString;

		public MigrationWithBlackJackAndHookers(string connectionString)
		{
			this.connectionString = connectionString;
		}

		private void QueryOperation(string operation, string tableName)
		{
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				string sqlScriptsDir = Path.Combine(Environment.CurrentDirectory, @"../DAL/Service/SqlScripts");
				string query = File.ReadAllText($"{sqlScriptsDir}/{operation}{tableName}.sql", Encoding.GetEncoding(1251));
				db.Execute(query);
			}
		}

		public void AlterTable(string tableName)
		{
			QueryOperation("Alter", tableName);
		}

		public void CreateTable(string tableName)
		{
			QueryOperation("Create", tableName);
		}

		public void InitiateTable(string tableName)
		{
			QueryOperation("Seed", tableName);
		}

		public bool IsExistTable(string tableName)
		{
			string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}'";
			using (IDbConnection db = new SqlConnection(connectionString))
			{
				int? countObj = db.Query<int>(query).FirstOrDefault();
				int count = countObj.Value;
				if (count != 0)
					return true;
			}
			return false;
		}
	}
}
