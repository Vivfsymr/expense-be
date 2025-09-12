using ExpenseBe.Core.Models;
using ExpenseBe.Data.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ExpenseBe.Data.Repositories
{
    public class WordRepository
    {
        private readonly IMongoCollection<Word> _words;
        public WordRepository(MongoDbContext context)
        {
            _words = context.Words;
        }

        public async Task<IEnumerable<Word>> GetAllAsync(string orderBy)
        {
            var query = _words.AsQueryable();
            List<Word> result;
            switch (orderBy?.ToLower())
            {
                case "alpha":
                    result = query.OrderBy(w => w.body).ToList();
                    break;
                case "beta":
                    result = query.OrderByDescending(w => w.body).ToList();
                    break;
                case "newest":
                    result = query.OrderByDescending(w => w.createAt).ToList();
                    break;
                case "oldest":
                    result = query.OrderBy(w => w.createAt).ToList();
                    break;
                case "random":
                    result = query.OrderBy(_ => Guid.NewGuid()).ToList();
                    break;
                default:
                    result = query.ToList();
                    break;
            }
            return await Task.FromResult(result);
        }

        public async Task InsertAsync(Word word)
        {
            await _words.InsertOneAsync(word);
        }
    }
}
