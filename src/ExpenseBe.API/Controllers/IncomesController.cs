using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using ExpenseBe.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ExpenseBe.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseBe.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IExcelExportService _excelExportService;

        public IncomesController(IIncomeService incomeService, IExcelExportService excelExportService)
        {
            _incomeService = incomeService;
            _excelExportService = excelExportService;
        }

        [Authorize]
        [HttpGet("getByQuery/{forUserId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Income>>>> GetByForUserId(string forUserId, [FromQuery] int? month, [FromQuery] int? year, [FromQuery] int? day)
        {
            try
            {
                var incomes = await _incomeService.GetIncomesByForUserIdAsync(forUserId, month, year, day);
                return Ok(new ApiResponse<IEnumerable<Income>>
                {
                    Success = true,
                    Message = "Incomes fetched successfully",
                    Data = incomes
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Income>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Income>()
                });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Income>>> GetById(string id)
        {
            try
            {
                var income = await _incomeService.GetIncomeByIdAsync(id);
                if (income == null)
                    return NotFound();
                return Ok(new ApiResponse<Income>
                {
                    Success = true,
                    Message = "Income fetched successfully",
                    Data = income
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Income>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Income>>> Create(Income income)
        {
            try
            {
                var createdIncome = await _incomeService.CreateIncomeAsync(income);
                return CreatedAtAction(nameof(GetById), new { id = createdIncome.Id }, new ApiResponse<Income>
                {
                    Success = true,
                    Message = "Income created successfully",
                    Data = createdIncome
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Income>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Income>>> Update(string id, Income income)
        {
            try
            {
                var updated = await _incomeService.UpdateIncomeAsync(id, income);
                if (updated == null)
                    return NotFound();
                return Ok(new ApiResponse<Income>
                {
                    Success = true,
                    Message = "Income updated successfully",
                    Data = income
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Income>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Income>>> Delete(string id)
        {
            try
            {
                var success = await _incomeService.DeleteIncomeAsync(id);
                if (!success)
                    return NotFound();
                return Ok(new ApiResponse<Income>
                {
                    Success = true,
                    Message = "Income deleted successfully",
                    Data = null
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<Income>
                {
                    Success = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet("export-excel/{forUserId}")]
        public async Task<IActionResult> ExportToExcelByForUserId(string forUserId, [FromQuery] int? month, [FromQuery] int? year)
        {
            try
            {
                var incomes = await _incomeService.GetIncomesByForUserIdAsync(forUserId, month, year);
                incomes = incomes.OrderBy(i => i.Date);
                var excelBytes = _excelExportService.ExportIncomesToExcel(incomes);
                
                var fileName = $"incomes_{forUserId}";
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