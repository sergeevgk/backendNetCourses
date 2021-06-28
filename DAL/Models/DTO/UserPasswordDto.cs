using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.DTO
{
	public class UserPasswordDto
	{
		public string Login { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
