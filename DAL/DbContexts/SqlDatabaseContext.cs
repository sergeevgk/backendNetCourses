using Common.Service;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DbContexts
{
	public class SqlDatabaseContext : IDatabaseContext
	{
		private IRepository<Factory> factoryRepository;
		private IRepository<Unit> unitRepository;
		private IRepository<Tank> tankRepository;
		private string connectionString;
		public SqlDatabaseContext()
		{

		}

		public IDatabaseContext UseConnection(string connectionString)
		{
			this.connectionString = connectionString;
			return this;
		}

		public async Task InitialiseDatabase()
		{
			IMigrationWithBlackJackAndHookers service = new MigrationWithBlackJackAndHookers(connectionString);
			if (!service.IsExistTable("Factory"))
			{
				service.CreateTable("Factory");
				service.InitiateTable("Factory");
			}
			if (!service.IsExistTable("Unit"))
			{
				service.CreateTable("Unit");
				service.AlterTable("Unit");
				service.InitiateTable("Unit");
			}
			if (!service.IsExistTable("Tank"))
			{
				service.CreateTable("Tank");
				service.AlterTable("Tank");
				service.InitiateTable("Tank");
			}
		}

		public IRepository<Factory> Factories
		{
			get
			{
				if (factoryRepository == null)
				{
					factoryRepository = new FactorySqlRepository(connectionString);
				}
				return factoryRepository;
			}
		}
		public IRepository<Unit> Units
		{
			get
			{
				if (unitRepository == null)
				{
					unitRepository = new UnitSqlRepository(connectionString);
				}
				return unitRepository;
			}
		}
		public IRepository<Tank> Tanks
		{
			get
			{
				if (tankRepository == null)
				{
					tankRepository = new TankSqlRepository(connectionString);
				}
				return tankRepository;
			}
		}
		public async Task SaveAsync()
		{
		}
		public async Task LoadAsync()
		{
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
				}
				this.disposed = true;
			}
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
