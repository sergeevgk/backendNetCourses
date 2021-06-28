using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models.DTO
{
	public class UserLoginDto
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
