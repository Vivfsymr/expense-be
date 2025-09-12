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

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Word> Words => _database.GetCollection<Word>("words");

    }
} 