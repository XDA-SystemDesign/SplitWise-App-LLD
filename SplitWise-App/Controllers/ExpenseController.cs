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
    public class ExpenseController : ControllerBase
    {
        private IExpenseValidationService expenseService;
        public ExpenseController(IExpenseValidationService expenseService)
        {
            this.expenseService = expenseService;
        }

        [HttpGet("GetSummary")]
        public IActionResult GetSummary([FromQuery]int id)
        {
            try
            {
                return Ok(expenseService.GetExpenseSummary(id));
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
        // POST api/<ExpenseController>
        [HttpPost]
        public IActionResult Post([FromBody] ExpenseRequestDTO value)
        {
            try
            {
                return Ok(expenseService.createExpense(value));
            } catch (ValidationException ex)
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
