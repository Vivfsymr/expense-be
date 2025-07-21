using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("{forUserId}")]
        public async Task<ActionResult<Summary>> GetSummary(string forUserId, [FromQuery] int year)
        {
            var summary = await _dashboardService.GetSummaryByUserAndYearAsync(forUserId, year);
            return Ok(summary);
        }
    }
} 