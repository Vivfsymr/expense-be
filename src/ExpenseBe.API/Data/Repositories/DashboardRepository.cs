using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using ExpenseBe.Data.Context;
using MongoDB.Driver;
using System.Globalization;

namespace ExpenseBe.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IMongoCollection<Expense> _expenses;
        private readonly IMongoCollection<Income> _incomes;
        private readonly IMongoCollection<Category> _categories;

        public DashboardRepository(MongoDbContext context)
        {
            _expenses = context.Expenses;
            _incomes = context.Incomes;
            _categories = context.Categories;
        }

        public async Task<Summary> GetSummaryByUserAndYearAsync(string forUserId, int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year + 1, 1, 1);

            // Lấy expenses và incomes của user trong năm
            var expenses = await _expenses.Find(e => e.ForUserId == forUserId && e.Date >= start && e.Date < end).ToListAsync();
            var incomes = await _incomes.Find(i => i.ForUserId == forUserId && i.Date >= start && i.Date < end).ToListAsync();
            var allCategories = await _categories.Find(_ => true).ToListAsync();

            var summary = new Summary
            {
                NameOfYear = year.ToString(),
                TotalYearAmountExpense = expenses.Sum(e => e.Amount),
                TotalYearAmountIncome = incomes.Sum(i => i.Amount),
                SummaryMonths = new List<SummaryMonth>()
            };

            for (int m = 1; m <= 12; m++)
            {
                var monthExpenses = expenses.Where(e => e.Date.Month == m).ToList();
                var monthIncomes = incomes.Where(i => i.Date.Month == m).ToList();
                var monthName = m.ToString("D2");

                var eachSummaryMonthOfCategories = allCategories
                    .Select(cat => new EachSummaryMonthOfCategory
                    {
                        NameOfCategory = cat.Name,
                        TotalCategoryAmountExpense = monthExpenses
                            .Where(e => e.Category == cat.Id || e.Category == cat.Name)
                            .Sum(e => e.Amount)
                    }).ToList();

                var summaryMonth = new SummaryMonth
                {
                    NameOfMonth = monthName,
                    TotalMonthAmountExpense = monthExpenses.Sum(e => e.Amount),
                    TotalMonthAmountIncome = monthIncomes.Sum(i => i.Amount),
                    TotalMonthAmountRealExpense = monthExpenses.Sum(e => e.Amount),
                    EachSummaryMonthOfCategories = eachSummaryMonthOfCategories
                };
                summary.SummaryMonths.Add(summaryMonth);
            }

            return summary;
        }
    }
} 