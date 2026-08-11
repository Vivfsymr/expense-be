using ExpenseBe.Core.Models;
using ExpenseBe.Data.Repositories;
using System;
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

        public async Task<WordListResult> GetWordsAsync(string? keyword, string? orderBy, int offset = 0, int limit = 50)
        {
            return await _wordRepository.GetAllAsync(keyword, orderBy, offset, limit);
        }

        public async Task<WordQuizResult> GetQuizAsync(
            string? keyword,
            string? orderBy,
            int offset = 0,
            int limit = 50)
        {
            limit = Math.Clamp(limit, 1, 50);
            offset = Math.Max(0, offset);
            var sort = string.IsNullOrWhiteSpace(orderBy) ? "newest" : orderBy;
            var source = await _wordRepository.GetAllAsync(keyword, sort, offset, limit);

            var items = new List<WordQuizItem>();
            foreach (var word in source.items)
            {
                var item = WordBodyParser.TryParseQuizItem(word);
                if (item == null)
                    continue;
                items.Add(item);
            }

            return new WordQuizResult
            {
                total = (int)source.total,
                items = items
            };
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
