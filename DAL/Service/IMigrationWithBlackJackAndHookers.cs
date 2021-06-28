using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Service
{
	public interface IMigrationWithBlackJackAndHookers
	{
		public bool IsExistTable(string tableName);

		public void CreateTable(string sqlQuery);

		public void AlterTable(string sqlQuery);

		public void InitiateTable(string sqlQuery);
	}
}
