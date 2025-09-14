using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using ExpenseBe.Data.Context;
using ExpenseBe.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseBe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly WordService _wordService;
        public WordsController(MongoDbContext context)
        {
            var repo = new WordRepository(context);
            _wordService = new WordService(repo);
        }

                [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> GetWords([FromQuery] string? keyword, [FromQuery] string? orderBy, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            var words = await _wordService.GetWordsAsync(keyword, orderBy, offset, limit);
            return Ok(words);
        }

        [HttpPost]
        public async Task<IActionResult> InsertWord([FromBody] Word word)
        {
            word.createAt = System.DateTime.UtcNow;
            await _wordService.InsertWordAsync(word);
            return Ok();
        }

        [HttpPost("form")]
        public async Task<IActionResult> InsertWordFromForm([FromForm] string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return BadRequest("Body is required");

            // Lấy từ đầu tiên trong dòng đầu tiên
            var firstLine = body.Split('\n', '\r')[0].Trim();
            var firstWord = firstLine.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim();

            // Kiểm tra đã tồn tại chưa (tìm word có body bắt đầu bằng từ này)
            var exists = await _wordService.ExistsByFirstWordAsync(firstWord);
            if (exists)
                return Conflict($"Từ đầu tiên '{firstWord}' đã được add trước đó!");

            var word = new Word
            {
                body = body,
                createAt = System.DateTime.UtcNow
            };
            await _wordService.InsertWordAsync(word);
            return Ok();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<object>>> GetWordSummaries([FromQuery] string? keyword, [FromQuery] string? orderBy, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            var words = await _wordService.GetWordsAsync(keyword, orderBy, offset, limit);
            var summaries = words.Select(w => new {
                w._id,
                body = GetFirstTwoSentences(w.body)
            });
            return Ok(summaries);
        }

        private static string GetFirstTwoSentences(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var sentences = text.Split(new[] {"\\n"}, System.StringSplitOptions.RemoveEmptyEntries);
            return string.Join(". ", sentences.Take(2)).Trim() + (sentences.Length > 2 ? "." : "");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> GetById(string id)
        {
            var word = await _wordService.GetByIdAsync(id);
            if (word == null)
                return NotFound();
            return Ok(word);
        }
    }
}
