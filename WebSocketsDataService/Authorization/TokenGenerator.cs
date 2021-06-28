using WebSocketsDataService.Models;
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

namespace WebSocketsDataService.Authorization
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly IConfiguration configuration;

		public TokenGenerator(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string GenerateJwtToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, user.Role.Name)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuthOptions:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddHours(Convert.ToDouble(configuration["JwtAuthOptions:ExpiresHours"]));

			var token = new JwtSecurityToken(
				claims: claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
