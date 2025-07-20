using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Summary> GetSummaryByUserAndYearAsync(string forUserId, int year)
        {
            return await _dashboardRepository.GetSummaryByUserAndYearAsync(forUserId, year);
        }
    }
} 