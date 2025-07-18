using ExpenseBe.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Interfaces
{
    public interface IIncomeRepository
    {
        Task<Income> CreateAsync(Income income);
        Task<Income> UpdateAsync(string id, Income income);
        Task<bool> DeleteAsync(string id);
        Task<Income?> GetByIdAsync(string id);
        Task<IEnumerable<Income>> GetByForUserIdAsync(string forUserId, int? month = null, int? year = null, int? day = null);
    }
} 