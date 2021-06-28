using WebSocketsDataService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Authorization
{
	public interface ITokenGenerator
	{
		public string GenerateJwtToken(User user);
	}
}
