using Microsoft.AspNetCore.Mvc;
using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Business.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SplitWise_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private ISettlementValidationService settlementService;
        public SettlementController(ISettlementValidationService settlementService)
        {
            this.settlementService = settlementService;
        }

        // POST api/<SettlementController>
        [HttpPost]
        public IActionResult Post([FromBody] SettlementDTO value)
        {
            try
            {
                this.settlementService.createASettlementRecord(value);
                return Ok("Created");
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Invalid Inputs provided - {ex.Message}");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex);
            }
        }
    }
}
