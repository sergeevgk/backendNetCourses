using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketsDataService.Models;

namespace WebSocketsDataService.Repository
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
