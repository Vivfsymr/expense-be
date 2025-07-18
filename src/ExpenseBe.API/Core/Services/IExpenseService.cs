using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(string userId);
        Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(string userId, int? month, int? year);
        Task<IEnumerable<Expense>> GetExpensesByForUserIdAsync(string forUserId);
        Task<Expense?> GetExpenseByIdAsync(string id);
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<bool> UpdateExpenseAsync(string id, Expense expense);
        Task<bool> DeleteExpenseAsync(string id);
        Task<decimal> GetRealExpensesAsync(string forUserId, int? month = null, int? year = null);
    }
} 