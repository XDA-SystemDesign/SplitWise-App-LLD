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
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        private IUserValidationService userService;
        public UserController(IUserValidationService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(this.userService.GetUser(id));
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
        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] UserRequestDTO value)
        {
            try
            {
                return Ok(this.userService.createUser(value));
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
