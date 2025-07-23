using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetByUserIdAsync(string userId, int? month, int? year);
        Task<Expense?> GetByIdAsync(string id);
        Task<Expense> CreateAsync(Expense expense);
        Task<bool> UpdateAsync(string id, Expense expense);
        Task<bool> DeleteAsync(string id);
        Task<decimal> GetRealExpensesAsync(string forUserId, int? month = null, int? year = null);
    }
} 