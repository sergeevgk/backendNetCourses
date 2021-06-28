using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6Patterns.Builder
{
	public interface IUserHttpServiceBuilder
	{
		IUserHttpServiceBuilder WithGetAllUsersAsync(IList<IUser> users);
		IUserHttpServiceBuilder WithGetAllBrigadesAsync(IList<BrigadeDto> brigades);
		IUserHttpServiceBuilder WithGetUserById(IUser user);
		IUserHttpServiceBuilder WithGetBrigadeById(BrigadeDto brigade);
		IUserHttpService Build();
	}
}
