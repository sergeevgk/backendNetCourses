using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DbContexts
{
    public interface IDatabaseContext 
    { 
        IRepository<Factory> Factories { get; }
        IRepository<Unit> Units { get; }
        IRepository<Tank> Tanks { get; }
        Task SaveAsync();
        Task LoadAsync();
        IDatabaseContext UseConnection(string connectionString);
        Task InitialiseDatabase();
    }
}
