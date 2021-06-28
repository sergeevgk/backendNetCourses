using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.DbContexts;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using DAL.Models.DTO;
using Microsoft.Extensions.Logging;
using WebAPI.Validation;
using AutoMapper;
using DAL.Repository;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitEventController : ControllerBase
    {
		private readonly ILogger<UnitEventController> logger;
		private readonly IUnitEventRepository unitEventRepository;
		private readonly UnitEventValidator validator;
		private readonly IMapper mapper;

		public UnitEventController(ILogger<UnitEventController> logger, 
									IUnitEventRepository unitEventRepository,
                                    UnitEventValidator validator,
                                    IMapper mapper)
        {
			this.logger = logger;
			this.unitEventRepository = unitEventRepository;
			this.validator = validator;
			this.mapper = mapper;
		}

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UnitEventDto>>> GetUnitEvents([FromQuery]int take, [FromQuery]int skip)
        {
            var entities = unitEventRepository.GetUnitEvents(take, skip);
            if (entities == null)
			{
                return NotFound("No entities awailiable");
			}
            return Ok(mapper.Map<IEnumerable<UnitEventDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnitEvent>> GetUnitEvent(int id)
        {
            try
            {
                var entity = await unitEventRepository.GetEntityById(id);
                if (entity == null)
                {
                    return NotFound($"UnitEvent with id '{id}' is not presented in database.");
                }
                return Ok(mapper.Map<UnitEventDto>(entity));
            }
            catch (Exception ex)
            {
                logger.LogError($"Internal server error on getting UnitEvent [id: {id}]: " + ex.Message);
                return Problem($"Internal server error on getting UnitEvent", statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitEvent(int id, UnitEventDto unitEvent) 
        {
            try
            {
                if (unitEvent == null)
                {
                    return BadRequest("Provide valid UnitEvent to update one in database.");
                }
                var validationResults = validator.Validate(unitEvent);
                if (!validationResults.IsValid)
                {
                    return BadRequest(validationResults.Errors.Select(error => error.ErrorMessage));
                }
                var existUnitEvent = mapper.Map<UnitEvent>(unitEvent);
                existUnitEvent.Id = id;
                var newUnitEvent = await unitEventRepository.UpdateEntity(existUnitEvent);
                if (newUnitEvent == null)
                {
                    throw new Exception("Repository error");
                }
                return Ok(mapper.Map<UnitEventDto>(newUnitEvent));
            }
            catch (Exception ex)
            {
                logger.LogError($"Internal server error on updating UnitEvent [id: {id}]: " + ex.Message);
                return Problem("Internal server error on updating UnitEvent", statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await unitEventRepository.DeleteEntity(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Internal server error on deleting unitEvent [id: {id}]: " + ex.Message);
                return Problem("Internal server error on deleting unitEvent", statusCode: 500);
            }
        }
    }
}
