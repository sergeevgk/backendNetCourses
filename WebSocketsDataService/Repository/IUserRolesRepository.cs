using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Models;

namespace WebSocketsDataService.Repository
{
	public interface IUserRolesRepository
	{
		Task<IList<UserRole>> GetRoles();
	}
}
