using Microsoft.AspNetCore.Mvc;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseBe.API.DTOs;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Status>>>> GetAll()
        {
            try
            {
                var statuses = await _statusService.GetAllStatusesAsync();
                return Ok(new ApiResponse<IEnumerable<Status>>
                {
                    Success = true,
                    Message = "Statuses fetched successfully",
                    Data = statuses
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse<IEnumerable<Status>>
                {
                    Success = false,
                    Message = e.Message,
                    Data = new List<Status>()
                });
            }
        }
    }
} 