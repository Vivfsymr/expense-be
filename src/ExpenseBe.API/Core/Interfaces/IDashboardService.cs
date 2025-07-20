using ExpenseBe.Core.Models;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<Summary> GetSummaryByUserAndYearAsync(string forUserId, int year);
    }
} 