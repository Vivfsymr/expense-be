using ExpenseBe.API.DTOs;
using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
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
        private readonly IAiWordGenerator _aiWordGenerator;

        public WordsController(WordService wordService, IAiWordGenerator aiWordGenerator)
        {
            _wordService = wordService;
            _aiWordGenerator = aiWordGenerator;
        }

        [HttpPost("bookmark/{id}")]
        public async Task<IActionResult> SetBookMark(string id, [FromQuery] bool value = true)
        {
            var result = await _wordService.SetBookMarkAsync(id, value);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<WordListResult>> GetWords([FromQuery] string? keyword, [FromQuery] string? orderBy, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            var result = await _wordService.GetWordsAsync(keyword, orderBy, offset, limit);
            return Ok(result);
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

        [HttpPost("ai")]
        public async Task<IActionResult> InsertWordFromAi([FromBody] AiWordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Word))
                return BadRequest("Word is required");

            var input = request.Word.Trim();
            var exists = await _wordService.ExistsByFirstWordAsync(input);
            if (exists)
                return Conflict($"Từ đầu tiên '{input}' đã được add trước đó!");

            string body;
            try
            {
                body = await _aiWordGenerator.GenerateAsync(input);
            }
            catch (System.InvalidOperationException ex)
            {
                return StatusCode(502, ex.Message);
            }

            if (string.IsNullOrWhiteSpace(body))
                return StatusCode(502, "Gemini returned empty content.");

            var word = new Word
            {
                body = body,
                createAt = System.DateTime.UtcNow
            };
            await _wordService.InsertWordAsync(word);
            return Ok(new { word._id, word.body });
        }

        [HttpPost("ai/lookup")]
        public async Task<IActionResult> LookupWordFromAi([FromBody] AiWordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Word))
                return BadRequest("Word is required");

            var input = request.Word.Trim();
            string body;
            try
            {
                body = await _aiWordGenerator.GenerateAsync(input);
            }
            catch (System.InvalidOperationException ex)
            {
                return StatusCode(502, ex.Message);
            }

            if (string.IsNullOrWhiteSpace(body))
                return StatusCode(502, "Gemini returned empty content.");

            return Ok(new { word = input, body });
        }

        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<object>>> GetWordSummaries([FromQuery] string? keyword, [FromQuery] string? orderBy, [FromQuery] int offset = 0, [FromQuery] int limit = 50)
        {
            var result = await _wordService.GetWordsAsync(keyword, orderBy, offset, limit);
            var summaries = result.items.Select(w => new {
                w._id,
                body = GetFirstTwoSentences(w.body)
            });
            return Ok(new { total = result.total, items = summaries });
        }

        [HttpGet("quiz")]
        public async Task<ActionResult<WordQuizResult>> GetQuiz(
            [FromQuery] string? keyword,
            [FromQuery] string? orderBy,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 50)
        {
            var result = await _wordService.GetQuizAsync(keyword, orderBy, offset, limit);
            return Ok(result);
        }

        private static string GetFirstTwoSentences(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var sentences = text.Split(new[] { "\\n" }, System.StringSplitOptions.RemoveEmptyEntries);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var word = await _wordService.GetByIdAsync(id);
            if (word == null)
                return NotFound();

            await _wordService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
