using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeService(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task<Income> CreateIncomeAsync(Income income)
        {
            return await _incomeRepository.CreateAsync(income);
        }

        public async Task<Income> UpdateIncomeAsync(string id, Income income)
        {
            return await _incomeRepository.UpdateAsync(id, income);
        }

        public async Task<bool> DeleteIncomeAsync(string id)
        {
            return await _incomeRepository.DeleteAsync(id);
        }

        public async Task<Income?> GetIncomeByIdAsync(string id)
        {
            return await _incomeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Income>> GetIncomesByForUserIdAsync(string forUserId, int? month = null, int? year = null, int? day = null)
        {
            return await _incomeRepository.GetByForUserIdAsync(forUserId, month, year, day);
        }
    }
} 