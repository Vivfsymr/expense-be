using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using ExpenseBe.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ExpenseBe.API.DTOs;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IExcelExportService _excelExportService;

        public ExpensesController(IExpenseService expenseService, IExcelExportService excelExportService)
        {
            _expenseService = expenseService;
            _excelExportService = excelExportService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Expense>>>> GetAll()
        {
            try
            {
                var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(new ApiResponse<IEnumerable<Expense>>
            {
                Success = true,
                Message = "Expenses fetched successfully",
                Data = expenses
            });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Expense>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Expense>()
                });
            }   
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Expense>>> GetById(string id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                    return NotFound();

            return Ok(new ApiResponse<Expense>
            {
                Success = true,
                Message = "Expense fetched successfully",
                Data = expense
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Expense>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpGet("getByQuery/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Expense>>>> GetByUserId(string userId, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesByUserIdAsync(userId, month, year);
                return Ok(new ApiResponse<IEnumerable<Expense>>
                {
                Success = true,
                Message = "Expenses fetched successfully",
                    Data = expenses
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Expense>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Expense>()
                });
            }
        }

        [HttpGet("for-user/{forUserId}")]
            public async Task<ActionResult<ApiResponse<IEnumerable<Expense>>>> GetByForUserId(string forUserId)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesByForUserIdAsync(forUserId);
                return Ok(new ApiResponse<IEnumerable<Expense>>
                {
                Success = true,
                Message = "Expenses fetched successfully",
                Data = expenses
            });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Expense>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Expense>()
                });
            }
        }

        [HttpPost]
            public async Task<ActionResult<ApiResponse<Expense>>> Create(Expense expense)
        {
            try
            {
                var createdExpense = await _expenseService.CreateExpenseAsync(expense);
                return CreatedAtAction(nameof(GetById), new { id = createdExpense.Id }, new ApiResponse<Expense>
                    {
                    Success = true,
                    Message = "Expense created successfully",
                    Data = createdExpense
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Expense>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Expense>>> Update(string id, Expense expense)
        {
            try
            {
                var success = await _expenseService.UpdateExpenseAsync(id, expense);
                if (!success)
                    return NotFound();

            return Ok(new ApiResponse<Expense>
            {
                Success = true,
                Message = "Expense updated successfully",
                    Data = expense
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Expense>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Expense>>> Delete(string id)
        {
            try
            {
                var success = await _expenseService.DeleteExpenseAsync(id);
                if (!success)
                    return NotFound();  

                return Ok(new ApiResponse<Expense>
                {
                    Success = true,
                    Message = "Expense deleted successfully",
                    Data = null
                }); 
            }
                catch (Exception e)
            {
                return BadRequest(new ApiResponse<Expense>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }
        
        [HttpGet("RealExpenses/{forUserId}")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetRealExpenses(string forUserId, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                var total = await _expenseService.GetRealExpensesAsync(forUserId, month, year);
                return Ok(new ApiResponse<decimal>
                {
                    Success = true,
                    Message = "Real expenses calculated successfully",
                    Data = total
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<decimal>
                {
                    Success = false,
                    Message = e.Message,
                    Data = 0
                });
            }
        }

        [HttpGet("export-excel/{forUserId}")]
        public async Task<IActionResult> ExportToExcelByForUserId(string forUserId, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesByUserIdAsync(forUserId, month, year);
                var excelBytes = await _excelExportService.ExportExpensesToExcel(expenses);
                
                var fileName = $"expenses_{forUserId}";
                if (month.HasValue && year.HasValue)
                {
                    fileName += $"_{year}_{month:D2}";
                }
                fileName += ".xlsx";
                
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }
    }
}