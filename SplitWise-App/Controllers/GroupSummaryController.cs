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
    public class GroupSummaryController : ControllerBase
    {
        private IGroupSummaryValidationService groupSummaryValidationService;
        public GroupSummaryController(IGroupSummaryValidationService groupSummaryValidationService)
        {
            this.groupSummaryValidationService = groupSummaryValidationService;
        }
        // GET api/<GroupSummaryController>/5
        [HttpGet]
        public IActionResult Get([FromQuery] GroupSummaryDTO groupSummary)
        {
            try
            {
                return Ok(this.groupSummaryValidationService.getSummaryOfAUserInAGroup(groupSummary));
            }
            catch (ValidationException e)
            {
                return BadRequest($"Invalid Inputs provided - {e.Message}");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex);
            }
        }
    }
}
