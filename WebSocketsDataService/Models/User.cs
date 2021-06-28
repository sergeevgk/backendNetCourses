using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Login { get; set; }
		public string RoleId { get; set; }
		public UserRole Role { get; set; }
		public string Name { get; set; }
		public string HashedPassword { get; set; }
	}
}
