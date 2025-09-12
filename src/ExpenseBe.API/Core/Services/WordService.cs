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

        public async Task<IEnumerable<Word>> GetWordsAsync(string orderBy)
        {
            return await _wordRepository.GetAllAsync(orderBy);
        }

        public async Task InsertWordAsync(Word word)
        {
            await _wordRepository.InsertAsync(word);
        }
    }
}
