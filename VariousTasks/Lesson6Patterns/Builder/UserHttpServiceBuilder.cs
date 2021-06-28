using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6Patterns.Builder
{
	public class UserHttpServiceBuilder : IUserHttpServiceBuilder
	{
		private Mock<IUserHttpService> mockService = new Mock<IUserHttpService>();
		public IUserHttpService Build()
		{
			return mockService.Object;
		}

		public IUserHttpServiceBuilder WithGetAllBrigadesAsync(IList<BrigadeDto> brigades)
		{
			mockService.Setup(x => x.GetAllBrigades()).Returns(Task.FromResult(brigades));
			return this;
		}

		public IUserHttpServiceBuilder WithGetAllUsersAsync(IList<IUser> users)
		{
			mockService.Setup(x => x.GetAllUsers()).Returns(Task.FromResult(users));
			return this;
		}

		public IUserHttpServiceBuilder WithGetBrigadeById(BrigadeDto brigade)
		{
			mockService.Setup(x => x.GetBrigadeById(brigade.Id)).Returns(Task.FromResult(brigade));
			return this;
		}

		public IUserHttpServiceBuilder WithGetUserById(IUser user)
		{
			mockService.Setup(x => x.GetUserById(user.Id)).Returns(Task.FromResult(user));
			return this;
		}
	}
}
