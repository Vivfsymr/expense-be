using ExpenseBe.Core.Models;
using ExpenseBe.Core.Services;
using ExpenseBe.Data.Context;
using ExpenseBe.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Word>>> GetWords([FromQuery] string orderBy)
        {
            var words = await _wordService.GetWordsAsync(orderBy);
            return Ok(words);
        }

        [HttpPost]
        public async Task<IActionResult> InsertWord([FromBody] Word word)
        {
            word.createAt = System.DateTime.UtcNow;
            await _wordService.InsertWordAsync(word);
            return Ok();
        }
    }
}
