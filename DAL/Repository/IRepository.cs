using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repository
{
	/// <summary>
	/// interface for general repository get / find methods
	/// </summary>
	public interface IRepository<T> where T : NamedDbObject
	{
		Task<IList<T>> GetEntities();
		Task<T> GetEntityById(int id);
		Task<T> CreateEntity(T entity);
		Task<T> UpdateEntity(T newEntity);
		Task DeleteEntity(int id);
	}
}
