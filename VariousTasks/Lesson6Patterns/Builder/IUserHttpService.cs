using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6Patterns.Builder
{
	public interface IUserHttpService
	{
		Task<IList<IUser>> GetAllUsers();
		Task<IList<BrigadeDto>> GetAllBrigades();
		Task<IUser> GetUserById(int id);
		Task<BrigadeDto> GetBrigadeById(int id);
	}

}
