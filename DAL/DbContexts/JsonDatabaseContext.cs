using DAL.Database;
using DAL.Models;
using DAL.Repository;
using DAL.Repository.JsonRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DbContexts
{
    public class JsonDatabaseContext: IDatabaseContext
    {
        private JsonDb db;
        private IRepository<Factory> factoryRepository;
        private IRepository<Unit> unitRepository;
        private IRepository<Tank> tankRepository;

        public JsonDatabaseContext()
        {

        }

        public IDatabaseContext UseConnection(string connectionString)
		{
            db = new JsonDb(connectionString);
            return this;
        }

        public async Task InitialiseDatabase()
		{
            await db.SeedData();
        }

        public IRepository<Factory> Factories
        {
            get
            {
                if (factoryRepository == null)
                {
                    factoryRepository = new FactoryJsonRepository(db);
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
                    unitRepository = new UnitJsonRepository(db);
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
                    tankRepository = new TankJsonRepository(db);
                }
                return tankRepository;
            }
        }
        public async Task SaveAsync()
        {
            await db.SaveAsync();
        }
        public async Task LoadAsync()
        {
            await db.ReadAsync();
        }
	}

}
