using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using ExpenseBe.Data.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Data.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IMongoCollection<Status> _statuses;

        public StatusRepository(MongoDbContext context)
        {
            _statuses = context.Statuses;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _statuses.Find(_ => true).ToListAsync();
        }
    }
} 