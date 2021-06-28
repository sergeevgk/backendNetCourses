using AutoMapper;
using DAL.Models;
using DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using WebAPI.Authorization;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> logger;
		private readonly IMapper mapper;
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly ITokenGenerator tokenGenerator;

		public UserController(ILogger<UserController> logger,
							IMapper mapper,
							UserManager<User> userManager,
							SignInManager<User> signInManager,
							RoleManager<IdentityRole> roleManager,
							ITokenGenerator tokenGenerator)
		{
			this.logger = logger;
			this.mapper = mapper;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
			this.tokenGenerator = tokenGenerator;
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[AllowAnonymous]
		[Route("auth")]
		[HttpPost]
		public async Task<ActionResult<string>> Login([FromBody] UserLoginDto model)
		{
			try
			{
				var signedUser = userManager.Users.FirstOrDefault(r => r.UserName == model.Login);
				var result = await signInManager.PasswordSignInAsync(signedUser, model.Password, false, false);

				if (result.Succeeded)
				{
					return Ok(await tokenGenerator.GenerateJwtToken(signedUser));
				}
				return BadRequest("Incorrect login or password");
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on login user [{model.Login}, {model.Password}]. " + ex.Message);
				return Problem("Internal server error on login user", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "ElevatedRights")]
		[Route("current")]
		[HttpGet]
		public ActionResult<UserDto> GetCurrentUser()
		{
			// HtpContext.User.Identity.Name
			string userId = User.FindFirst("sub")?.Value;
			var user = userManager.Users.FirstOrDefault(r => r.Id == userId);
			if (user == null)
			{
				return NotFound($"Current user cannot be identified.");
			}
			return mapper.Map<UserDto>(user);
		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "ElevatedRights")]
		[Route("password/update")]
		[HttpPost]
		public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordDto model)
		{
			string userId = User.FindFirst("sub")?.Value;
			var user = userManager.Users.FirstOrDefault(r => r.Id == userId);
			if (user == null)
			{
				return NotFound($"User with login {user.UserName} not found.");
			}
			var result = await signInManager.PasswordSignInAsync(user, model.CurrentPassword, false, false);
			if (result.Succeeded)
			{
				user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.NewPassword);
				var updateResult = await userManager.UpdateAsync(user);
				if (updateResult.Succeeded)
				{
					return Ok("Password updated successfully");
				}
				logger.LogError($"Error on updating password for user '{user.UserName}'. " + updateResult.Errors);
				return Problem($"Error on updating password for user '{user.UserName}'", statusCode: 500);
			}
			return BadRequest($"Provided pair login - password is incorrect.");
		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "RequireAdministratorRole")]
		[Route("all")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDto>>> Get()
		{
			try
			{
				var users = await userManager.Users.ToArrayAsync();
				if (users == null)
				{
					return NotFound("Users not exist");
				}
				return Ok(mapper.Map<IEnumerable<UserDto>>(users));
			}
			catch (Exception ex)
			{
				logger.LogError("Internal server error on getting users. " + ex.Message);
				return Problem("Internal server error on getting users");
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(int id)
		{
			throw new NotImplementedException();

		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "RequireAdministratorRole")]
		[HttpPost]
		public async Task<ActionResult<UserDto>> Post(UserCreateUpdateDto model)
		{
			try
			{
				var existUser = await userManager.FindByNameAsync(model.UserName);
				if (existUser != null)
				{
					return BadRequest($"User with login {model.UserName} already exists.");
				}
				var existUserEmail = await userManager.FindByEmailAsync(model.Email);
				if (existUserEmail != null)
				{
					return BadRequest($"User with email {model.Email} already exists.");
				}
				var newUser = mapper.Map<User>(model);
				newUser.PasswordHash = userManager.PasswordHasher.HashPassword(newUser, model.Password);
				var result = await userManager.CreateAsync(newUser);
				if (!result.Succeeded)
				{
					throw new Exception("UserManager create user action failed. " + result.Errors);
				}

				var createdUser = await userManager.FindByNameAsync(model.UserName);
				var isRoleExist = await roleManager.RoleExistsAsync(model.Role);
				if (!isRoleExist)
				{
					return BadRequest($"Role {model.Role} does not exist.");
				}
				result = await userManager.AddToRoleAsync(createdUser, model.Role);
				if (!result.Succeeded)
				{
					throw new Exception("UserManager add user to role action failed. " + result.Errors);
				}
				return Ok(mapper.Map<UserDto>(newUser));
			}
			catch (Exception ex)
			{
				logger.LogError("Internal server error on creating user [{model.UserName}, {model.Email}]. " + ex.Message);
				return Problem("Internal server error on creating user");
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, User user)
		{
			throw new NotImplementedException();

		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			throw new NotImplementedException();

		}
	}
}
