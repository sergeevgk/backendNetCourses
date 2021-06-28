using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DTO
{
	public class UserCreateUpdateDto
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }

	}
}
