using AutoMapper;
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
	public class UnitController : ControllerBase
	{
		private readonly ILogger<UnitController> logger;
		private readonly IUnitRepository unitsRepository;
		private readonly IFactoryRepository factoriesRepository;
		private readonly UnitValidator validator;
		private readonly IMapper mapper;

		public UnitController(ILogger<UnitController> logger,
								IUnitRepository unitsRepository,
								IFactoryRepository factoriesRepository,
								UnitValidator validator,
								IMapper mapper)
		{
			this.logger = logger;
			this.unitsRepository = unitsRepository;
			this.factoriesRepository = factoriesRepository;
			this.validator = validator;
			this.mapper = mapper;
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Route("all")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UnitDto>>> Get()
		{
			try
			{
				var units = await unitsRepository.GetEntities();
				if (units == null)
				{
					return NotFound("There are no units.");
				}
				// add mapper
				return Ok(mapper.Map<IList<UnitDto>>(units));
			}
			catch (Exception ex)
			{
				logger.LogError("Internal server error on getting units: " + ex.Message);
				return Problem("Internal server error on getting units:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<UnitWithTanksDto>> Get(int id)
		{
			try
			{
				var unit = await unitsRepository.GetUnitWithTanks(id);
				if (unit == null)
				{
					return NotFound($"Unit with id '{id}' is not presented in database.");
				}
				return Ok(mapper.Map<UnitWithTanksDto>(unit));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on getting unit [id: {id}]: " + ex.Message);
				return Problem($"Internal server error on getting unit:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Route("factory/{factoryId}")]
		[HttpPost]
		public async Task<IActionResult> Post(int factoryId, UnitCreateUpdateDto unit)
		{
			try
			{
				if (unit == null)
				{
					return BadRequest("Provide valid unit to create one in database.");
				}
				var validationResults = validator.Validate(unit);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var factory = await factoriesRepository.GetEntityById(factoryId);
				if (factory == null)
					return NotFound($"Factory with id {factoryId} was not found.");
				var newUnit = mapper.Map<Unit>(unit);
				newUnit.FactoryId = factoryId;
				var createdUnit = await unitsRepository.CreateEntity(newUnit);
				if (createdUnit == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<UnitDto>(newUnit));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on creating unit. " + ex.Message);
				return Problem("Internal server error on creating unit", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, UnitCreateUpdateDto unit)
		{
			try
			{
				if (unit == null)
				{
					return BadRequest("Provide valid unit to update one in database.");
				}
				var validationResults = validator.Validate(unit);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var existUnit = mapper.Map<Unit>(unit);
				existUnit.Id = id;
				var newUnit = await unitsRepository.UpdateEntity(existUnit);
				if (newUnit == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<UnitDto>(newUnit));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on updating unit [id: {id}]: " + ex.Message);
				return Problem("Internal server error on updating unit:", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await unitsRepository.DeleteEntity(id);
				// delete tanks also

				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on deleting unit [id: {id}]: " + ex.Message);
				return Problem("Internal server error on deleting unit:", statusCode: 500);
			}
		}
	}
}
