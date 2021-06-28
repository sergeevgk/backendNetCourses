using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Authorization
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly IConfiguration configuration;
		private readonly UserManager<User> userManager;

		public TokenGenerator(IConfiguration configuration, UserManager<User> userManager)
		{
			this.configuration = configuration;
			this.userManager = userManager;
		}

		public async Task<string> GenerateJwtToken(User user)
		{
			var userRoles = await userManager.GetRolesAsync(user);
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, userRoles?.FirstOrDefault() ?? "")
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuthOptions:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddMinutes(1);//AddHours(Convert.ToDouble(configuration["JwtAuthOptions:ExpiresHours"]));

			var token = new JwtSecurityToken(
				claims: claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		private static T Sum<T>(T num) where T : struct
		{
			T sum = default(T);
			for (T n = default(T); n < num; n++)
				sum += n;
			return sum;
		}
	}
}
