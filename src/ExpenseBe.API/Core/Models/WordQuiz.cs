using System.Collections.Generic;

namespace ExpenseBe.Core.Models
{
    public class WordQuizItem
    {
        public string id { get; set; } = string.Empty;
        public string english { get; set; } = string.Empty;
        public string vietnameseHint { get; set; } = string.Empty;
        public string? partOfSpeech { get; set; }
        public List<string> acceptedAnswers { get; set; } = new();
    }

    public class WordQuizResult
    {
        public int total { get; set; }
        public List<WordQuizItem> items { get; set; } = new();
    }
}
