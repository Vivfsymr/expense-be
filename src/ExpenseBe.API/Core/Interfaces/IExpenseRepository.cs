using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<IEnumerable<Expense>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Expense>> GetByUserIdAsync(string userId, int? month, int? year);
        Task<IEnumerable<Expense>> GetByForUserIdAsync(string forUserId);
        Task<Expense?> GetByIdAsync(string id);
        Task<Expense> CreateAsync(Expense expense);
        Task<bool> UpdateAsync(string id, Expense expense);
        Task<bool> DeleteAsync(string id);
    }
} 