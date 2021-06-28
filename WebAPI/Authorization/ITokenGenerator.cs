using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Authorization
{
	public interface ITokenGenerator
	{
		public Task<string> GenerateJwtToken(User user);
	}
}
