using DAL.Models;
using DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
	public interface IUserRepository
	{
		Task<IList<User>> GetUsers();
		Task<User> GetUserById(string id);
		Task<User> GetUserByLogin(string login);
		Task<User> CreateUser(User entity);
		Task<User> UpdateUser(User newEntity);
		Task DeleteUser(string id);
	}
}
