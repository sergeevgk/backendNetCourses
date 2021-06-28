using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Lesson6Patterns.Builder;

namespace TestProject
{
	public class MockTests
	{
		[Fact]
		public async void MockBuilderTest()
		{
			var users = new[]
			{
				new User { Id = 1 },
				new User { Id = 2 },
				new User { Id = 3 },
				new User { Id = 4 }
			};

			var brigades = new[]
			{
				new BrigadeDto { Id = 1 },
				new BrigadeDto { Id = 2 },
				new BrigadeDto { Id = 3 }
			};

			var service = new UserHttpServiceBuilder()
				.WithGetAllBrigadesAsync(brigades)
				.WithGetAllUsersAsync(users)
				.WithGetBrigadeById(brigades.First())
				.WithGetUserById(users.First())
				.Build();

			Assert.Equal(await service.GetUserById(1), users.First());
			Assert.Equal(await service.GetBrigadeById(1), brigades.First());

			Assert.Equal(await service.GetAllUsers(), users);
			Assert.Equal(await service.GetAllBrigades(), brigades);
		}
	}
}
