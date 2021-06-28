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
	public class FactoryController : ControllerBase
	{
		private readonly ILogger<FactoryController> logger;
		private readonly IFactoryRepository factoriesRepository;
		private readonly FactoryValidator validator;
		private readonly IMapper mapper;

		public FactoryController(ILogger<FactoryController> logger,
									IFactoryRepository factoriesRepository,
									FactoryValidator validator,
									IMapper mapper)
		{
			this.logger = logger;
			this.factoriesRepository = factoriesRepository;
			this.validator = validator;
			this.mapper = mapper;
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Route("all")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FactoryDto>>> Get()
		{
			try
			{
				var factories = await factoriesRepository.GetEntities();
				if (factories == null)
				{
					return NotFound("There are no factories.");
				}
				return Ok(mapper.Map<IList<FactoryDto>>(factories));
			}
			catch (Exception ex)
			{
				logger.LogError("Internal server error on getting factories: " + ex.Message);
				return Problem("Internal server error on getting factories", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<FactoryWithUnitsDto>> Get(int id)
		{
			try
			{
				var factory = await factoriesRepository.GetFactoryWithUnits(id);
				if (factory == null)
				{
					return NotFound($"Factory with id '{id}' is not presented in database.");
				}
				return Ok(mapper.Map<FactoryWithUnitsDto>(factory));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on getting factory [id: {id}]: " + ex.Message);
				return Problem($"Internal server error on getting factory", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] FactoryCreateUpdateDto factory)
		{
			try
			{
				if (factory == null)
				{
					return BadRequest("Provide valid factory to create one in database.");
				}
				var validationResults = validator.Validate(factory);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var newFactory = await factoriesRepository.CreateEntity(mapper.Map<Factory>(factory));
				if (newFactory == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<FactoryDto>(newFactory));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on creating factory. " + ex.Message);
				return Problem("Internal server error on creating factory", statusCode: 500);
			}
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, FactoryCreateUpdateDto factory)
		{
			try
			{
				if (factory == null)
				{
					return BadRequest("Provide valid factory to update one in database.");
				}
				var validationResults = validator.Validate(factory);
				if (!validationResults.IsValid)
				{
					return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
				}
				var existFactory = mapper.Map<Factory>(factory);
				existFactory.Id = id;
				var newFactory = await factoriesRepository.UpdateEntity(existFactory);
				if (newFactory == null)
				{
					throw new Exception("Repository error");
				}
				return Ok(mapper.Map<FactoryDto>(newFactory));
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on updating factory [id: {id}]: " + ex.Message);
				return Problem("Internal server error on updating factory", statusCode: 500);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await factoriesRepository.DeleteEntity(id);
				// delete units also?
				return Ok();
			}
			catch (Exception ex)
			{
				logger.LogError($"Internal server error on deleting factory [id: {id}]: " + ex.Message);
				return Problem("Internal server error on deleting factory", statusCode: 500);
			}
		}
	}
}
