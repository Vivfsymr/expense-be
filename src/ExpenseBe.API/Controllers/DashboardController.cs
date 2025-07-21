using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseBe.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("{forUserId}")]
        public async Task<ActionResult<Summary>> GetSummary(string forUserId, [FromQuery] int year)
        {
            var summary = await _dashboardService.GetSummaryByUserAndYearAsync(forUserId, year);
            return Ok(summary);
        }
    }
} 