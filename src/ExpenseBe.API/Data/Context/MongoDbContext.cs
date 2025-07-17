using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ExpenseBe.Core.Models;

namespace ExpenseBe.Data.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var databaseName = configuration["MongoDB:DatabaseName"] ?? "ExpenseDB";
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Expense> Expenses => _database.GetCollection<Expense>("expenses");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
        public IMongoCollection<Status> Statuses => _database.GetCollection<Status>("statuses");
    }
} 