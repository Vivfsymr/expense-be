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

        public async Task<IEnumerable<Word>> GetAllAsync(string? keyword, string? orderBy, int offset = 0, int limit = 50)
        {
            var filter = string.IsNullOrWhiteSpace(keyword)
                ? Builders<Word>.Filter.Empty
                : Builders<Word>.Filter.Regex(w => w.body, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            
            var sort = orderBy?.ToLower() switch
            {
                "alpha" => Builders<Word>.Sort.Ascending(w => w.body),
                "beta" => Builders<Word>.Sort.Descending(w => w.body),
                "newest" => Builders<Word>.Sort.Descending(w => w.createAt),
                "oldest" => Builders<Word>.Sort.Ascending(w => w.createAt),
                "random" => null, // Random sẽ xử lý riêng
                _ => Builders<Word>.Sort.Descending(w => w.createAt)
            };

            if (orderBy?.ToLower() == "random")
            {
                // Random: lấy tất cả rồi shuffle (chỉ nên dùng với ít data)
                var allResults = await _words.Find(filter).ToListAsync();
                return allResults.OrderBy(_ => Guid.NewGuid())
                    .Skip(offset)
                    .Take(limit);
            }

            var result = await _words.Find(filter)
                .Sort(sort)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();
            
            return result;
        }

        public async Task InsertAsync(Word word)
        {
            await _words.InsertOneAsync(word);
        }

        public async Task<bool> ExistsByFirstWordAsync(string firstWord)
        {
            var filter = Builders<Word>.Filter.Regex(w => w.body, new MongoDB.Bson.BsonRegularExpression($"^{System.Text.RegularExpressions.Regex.Escape(firstWord)}\\b", "i"));
            return await _words.Find(filter).AnyAsync();
        }

        public async Task<Word?> GetByIdAsync(string id)
        {
            try
            {
                if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                    return null;
                    
                var filter = Builders<Word>.Filter.Eq(w => w._id, id);
                return await _words.Find(filter).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
