using ExpenseBe.Core.Models;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<Summary> GetSummaryByUserAndYearAsync(string forUserId, int year);
    }
} 