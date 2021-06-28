using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsDataService.Authorization;
using WebSocketsDataService.Models;
using WebSocketsDataService.Models.DTO;
using WebSocketsDataService.Repository;

namespace WebSocketsDataService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private readonly ILogger<UserController> logger;
		private readonly IUserRepository userRepository;
		private readonly IUserRolesRepository userRolesRepository;
		private readonly IPasswordHasher<User> passwordHasher;
		private readonly ITokenGenerator tokenGenerator;
		private readonly IMapper mapper;

		public UserController(ILogger<UserController> logger,
							IUserRepository userRepository,
							IUserRolesRepository userRolesRepository,
							IPasswordHasher<User> passwordHasher,
							ITokenGenerator tokenGenerator,
							IMapper mapper)
		{
			this.logger = logger;
			this.userRepository = userRepository;
			this.userRolesRepository = userRolesRepository;
			this.passwordHasher = passwordHasher;
			this.tokenGenerator = tokenGenerator;
			this.mapper = mapper;
		}

		[Authorize]
		[HttpGet("all")]
		public ActionResult<IEnumerable<User>> GetUsers()
		{
			return Ok(userRepository.GetUsers());
		}

		[HttpGet("current")]
		public ActionResult<User> GetCurrentUser()
		{
			return Ok();
		}

		[HttpPost("auth")]
		[AllowAnonymous]
		public async Task<ActionResult<string>> Login([FromBody] UserLoginDto loginModel)
		{
			try
			{
				var existUser = await userRepository.GetUserByLogin(loginModel.Login);
				if (existUser == null)
				{
					return Unauthorized("123");
				}
				var verificationPasswordResult = passwordHasher.VerifyHashedPassword(existUser, existUser.HashedPassword, loginModel.Password);
				if (verificationPasswordResult != PasswordVerificationResult.Success)
				{
					return Unauthorized("321");
				}
				var token = tokenGenerator.GenerateJwtToken(existUser);
				return Ok(token);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "RequireAdministratorRole")]
		[HttpPost]
		public async Task<ActionResult<User>> Create([FromBody]UserCreateDto model)
		{
			try
			{
				var existUser = await userRepository.GetUserByLogin(model.Login);
				if (existUser != null)
				{
					return BadRequest($"User with login {model.Login} already exist");
				}
				var user = mapper.Map<User>(model);
				user.HashedPassword = passwordHasher.HashPassword(user, model.Password);
				var roles = await userRolesRepository.GetRoles();
				var userRole = roles.Where(r => r.Name == model.RoleName).FirstOrDefault();
				if (userRole == null)
				{
					return BadRequest($"User with role {model.RoleName} can not be created");
				}
				user.Role = userRole;
				user.RoleId = userRole.Id;
				var resultUser = await userRepository.CreateUser(user);
				if (resultUser == null)
				{
					return Problem(statusCode: 500, detail: "Internal server error on creating user");
				}
				return Ok(resultUser);
			}
			catch (Exception ex)
			{
				return Problem(statusCode: 500, detail: ex.Message);
			}
		}

		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Policy = "ElevatedRights")]
		[Route("password/update")]
		[HttpPost]
		public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordDto model)
		{
			var currentUserId = User.FindFirst("sub")?.Value;
			var currentUser = await userRepository.GetUserById(currentUserId);
			if (currentUser == null)
			{
				return Unauthorized($"Current user can not be identified.");
			}
			var result = passwordHasher.VerifyHashedPassword(currentUser, currentUser.HashedPassword, model.CurrentPassword);
			if (result != PasswordVerificationResult.Success)
			{
				return BadRequest($"Provided pair login - password is incorrect.");
			}
			currentUser.HashedPassword = passwordHasher.HashPassword(currentUser, model.NewPassword);
			var updatedUser = await userRepository.UpdateUser(currentUser);
			if (updatedUser == null)
			{
				logger.LogError($"Error on updating password for user '{currentUser.Login}'.");
				return Problem($"Error on updating password for user '{currentUser.Login}'", statusCode: 500);
			}
			return Ok("Password updated successfully");
		}

		[HttpDelete]
		public ActionResult Delete(int id)
		{
			return View();
		}

	}
}
