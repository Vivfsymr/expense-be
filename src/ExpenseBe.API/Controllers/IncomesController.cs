using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ExpenseBe.API.DTOs;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

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
    }
} 