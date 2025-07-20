using ExpenseBe.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public interface IIncomeService
    {
        Task<Income> CreateIncomeAsync(Income income);
        Task<Income> UpdateIncomeAsync(string id, Income income);
        Task<bool> DeleteIncomeAsync(string id);
        Task<Income?> GetIncomeByIdAsync(string id);
        Task<IEnumerable<Income>> GetAllIncomesAsync();
        Task<IEnumerable<Income>> GetIncomesByForUserIdAsync(string forUserId, int? month = null, int? year = null, int? day = null);
    }
} 