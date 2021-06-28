using AutoMapper;
using DAL.DbContexts;
using DAL.Models;
using DAL.Models.DTO;
using DAL.Repository;
using DAL.Repository.EFCoreRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Validation;

namespace WebAPI.Controllers
{
	[ApiController]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[Authorize(Policy = "RequireManagerOrAdmin")]
	[Route("api/[controller]")]
	public class TankController : ControllerBase
	{
		private readonly ILogger<TankController> logger;
		private readonly ITankRepository tanksRepository;
		private readonly IUnitRepository unitsRepository;
		private readonly TankValidator validator;
		private readonly IMapper mapper;

		public TankController(ILogger<TankController> logger,
								ITankRepository tanksRepository,
								IUnitRepository unitsRepository,
								TankValidator validator,
								IMapper mapper)
		{
			this.logger = logger;
			this.tanksRepository = tanksRepository;
			this.unitsRepository = unitsRepository;
			this.validator = validator;
			this.mapper = mapper;
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Route("all")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TankDto>>> Get()
		{
			try
			{
				var tanks = await tanksRepository.GetEntities();
				if (tanks == null)
				{
					return NotFound("There are no tanks.");
				}
				return Ok(mapper.Map<IList<TankDto>>(tanks));
			}
			catch (Exception ex)
			{
				logger.LogError("Internal server error on getting tanks: " + ex.Message);
				return Problem("Internal server error on getting tanks:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<TankDto>> Get(int id)
		{
			try
			{
				var tank = await tanksRepository.GetEntityById(id);
				if (tank == null)
				{
					return NotFound($"Tank with id '{id}' is not presented in database.");
				}
				return Ok(mapper.Map<TankDto>(tank));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on getting tank [id: {id}]: " + ex.Message);
				return Problem("Internal server error on getting tank:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Route("unit/{unitId}")]
		[HttpPost]
		public async Task<IActionResult> Post(int unitId, TankCreateUpdateDto tank)
		{
			try
			{
				if (tank == null)
				{
					return BadRequest("Provide valid tank to create one in database.");
				}
				var validationResults = validator.Validate(tank);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var unit = await unitsRepository.GetEntityById(unitId);
				if (unit == null)
					return NotFound($"Unit with id {unitId} was not found.");
				var newTank = mapper.Map<Tank>(tank);
				newTank.UnitId = unitId;
				var createdTank = await tanksRepository.CreateEntity(newTank);
				if (createdTank == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<TankDto>(newTank));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on creating tank. " + ex.Message);
				return Problem("Internal server error on creating tank", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, TankCreateUpdateDto tank)
		{
			try
			{
				if (tank == null)
				{
					return BadRequest("Provide valid tank to update one in database.");
				}
				var validationResults = validator.Validate(tank);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var existTank = mapper.Map<Tank>(tank);
				existTank.Id = id;
				var newTank = await tanksRepository.UpdateEntity(existTank);
				if (newTank == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<TankCreateUpdateDto>(newTank));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on updating tank [id: {id}]: " + ex.Message);
				return Problem("Internal server error on updating tank:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await tanksRepository.DeleteEntity(id);
				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on deleting tank [id: {id}]: " + ex.Message);
				return Problem("Internal server error on deleting tank:", statusCode: 500);
			}
		}
	}
}
