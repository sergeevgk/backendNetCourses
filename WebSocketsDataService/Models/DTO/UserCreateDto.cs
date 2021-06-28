using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.DTO
{
	public class UserCreateDto
	{
		public string Id { get; set; }
		public string Login{ get; set; }
		public string Name{ get; set; }
		public string Password{ get; set; }
		public string RoleName{ get; set; }
	}
}
