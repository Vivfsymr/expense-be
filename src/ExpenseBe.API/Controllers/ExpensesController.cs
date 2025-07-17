using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAll()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetById(string id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetByUserId(string userId, [FromQuery] int? month, [FromQuery] int? year)
        {
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId, month, year);
            return Ok(expenses);
        }

        [HttpGet("for-user/{forUserId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetByForUserId(string forUserId)
        {
            var expenses = await _expenseService.GetExpensesByForUserIdAsync(forUserId);
            return Ok(expenses);
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Create(Expense expense)
        {
            var createdExpense = await _expenseService.CreateExpenseAsync(expense);
            return CreatedAtAction(nameof(GetById), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Expense expense)
        {
            var success = await _expenseService.UpdateExpenseAsync(id, expense);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _expenseService.DeleteExpenseAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}