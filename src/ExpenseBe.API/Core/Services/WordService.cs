using ExpenseBe.Core.Models;
using ExpenseBe.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseBe.Core.Services
{
    public class WordService
    {
        private readonly WordRepository _wordRepository;
        public WordService(WordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public async Task<IEnumerable<Word>> GetWordsAsync(string? keyword, string? orderBy, int offset = 0, int limit = 50)
        {
            return await _wordRepository.GetAllAsync(keyword, orderBy, offset, limit);
        }

        public async Task InsertWordAsync(Word word)
        {
            await _wordRepository.InsertAsync(word);
        }

        public async Task<bool> ExistsByFirstWordAsync(string firstWord)
        {
            return await _wordRepository.ExistsByFirstWordAsync(firstWord);
        }

        public async Task<Word?> GetByIdAsync(string id)
        {
            return await _wordRepository.GetByIdAsync(id);
        }

        public async Task<bool> SetBookMarkAsync(string id, bool value)
        {
            return await _wordRepository.SetBookMarkAsync(id, value);
        }
        
        public async Task DeleteByIdAsync(string id)
        {
            await _wordRepository.DeleteByIdAsync(id);
        }
    }
}
