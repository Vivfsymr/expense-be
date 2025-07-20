using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using ExpenseBe.Data.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ExpenseBe.Data.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly IMongoCollection<Income> _incomes;

        public IncomeRepository(MongoDbContext context)
        {
            _incomes = context.Incomes;
        }

        public async Task<Income> CreateAsync(Income income)
        {
            await _incomes.InsertOneAsync(income);
            return income;
        }

        public async Task<Income> UpdateAsync(string id, Income income)
        {
            income.UpdatedAt = DateTime.UtcNow;
            var result = await _incomes.ReplaceOneAsync(x => x.Id == id, income);
            return income;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _incomes.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<Income?> GetByIdAsync(string id)
        {
            return await _incomes.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Income>> GetAllAsync()
        {
            return await _incomes.Find(_ => true).SortByDescending(x => x.Date).ToListAsync();
        }

        public async Task<IEnumerable<Income>> GetByForUserIdAsync(string forUserId, int? month = null, int? year = null, int? day = null)
        {
            var filter = Builders<Income>.Filter.Eq(x => x.ForUserId, forUserId);
            if (month.HasValue && year.HasValue)
            {
                filter = Builders<Income>.Filter.And(filter, Builders<Income>.Filter.Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value));
            }
            if (day.HasValue)
            {
                filter = Builders<Income>.Filter.And(filter, Builders<Income>.Filter.Where(x => x.Date.Day == day.Value));
            }
            return await _incomes.Find(filter).SortByDescending(x => x.Date).ToListAsync();
        }
    }
} 