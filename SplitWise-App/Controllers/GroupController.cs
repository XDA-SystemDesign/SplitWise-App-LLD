using Microsoft.AspNetCore.Mvc;
using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Business.Validations;
using SplitWise_Models;
using SplitWise_Services.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SplitWise_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        // GET: api/<GroupController>

        private IGroupValidationService groupService;
        public GroupController(IGroupValidationService groupService)
        {
            this.groupService = groupService;
        }

        // GET api/<GroupController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(groupService.GetGroupByGroupId(id));
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

        [HttpPost("CreateGroupAlongWithUsers")]
        public IActionResult CreateGroupAlongWithUsers([FromBody] GroupDTO groupDTO)
        {
            if (!ModelState.IsValid) throw new ValidationException("Invalid inputs provided"); 
            try
            {
                var result = this.groupService.createGroupFromDTO(groupDTO);
                var jsonString = JsonSerializer.Serialize(result, result.GetType());
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest($"Invalid Inputs provided - {ex.Message}");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
        // POST api/<GroupController>
        [HttpPost]
        public IActionResult Post([FromBody] Group group)
        {
            try
            {
                return Ok(groupService.createGroup(group));
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
