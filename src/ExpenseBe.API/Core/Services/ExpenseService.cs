using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(string userId, int? month, int? year)
        {
            return await _expenseRepository.GetByUserIdAsync(userId, month, year);
        }
       

        public async Task<Expense?> GetExpenseByIdAsync(string id)
        {
            return await _expenseRepository.GetByIdAsync(id);
        }

        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            return await _expenseRepository.CreateAsync(expense);
        }

        public async Task<bool> UpdateExpenseAsync(string id, Expense expense)
        {
            return await _expenseRepository.UpdateAsync(id, expense);
        }

        public async Task<bool> DeleteExpenseAsync(string id)
        {
            return await _expenseRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetRealExpensesAsync(string forUserId, int? month = null, int? year = null)
        {
            return await _expenseRepository.GetRealExpensesAsync(forUserId, month, year);
        }
    }
} 