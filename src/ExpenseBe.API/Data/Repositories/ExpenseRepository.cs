using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using ExpenseBe.Data.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Data.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IMongoCollection<Expense> _expenses;

        public ExpenseRepository(MongoDbContext context)
        {
            _expenses = context.Expenses;
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            return await _expenses.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(string userId)
        {
            return await _expenses.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(string userId, int? month, int? year)
        {
            var filter = Builders<Expense>.Filter.Eq(x => x.ForUserId, userId);
            if (month.HasValue && year.HasValue)
            {
                var monthFilter = Builders<Expense>.Filter.Where(x => x.Date.Month == month.Value && x.Date.Year == year.Value);
                filter = Builders<Expense>.Filter.And(filter, monthFilter);
            }
            return await _expenses.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetByForUserIdAsync(string forUserId)
        {
            return await _expenses.Find(x => x.ForUserId == forUserId).ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(string id)
        {
            return await _expenses.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            await _expenses.InsertOneAsync(expense);
            return expense;
        }

        public async Task<bool> UpdateAsync(string id, Expense expense)
        {
            expense.UpdatedAt = System.DateTime.UtcNow;
            var result = await _expenses.ReplaceOneAsync(x => x.Id == id, expense);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _expenses.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
} 