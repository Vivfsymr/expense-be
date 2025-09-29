using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ExpenseBe.Core.Models
{
    public class WordListResult
    {
        public long total { get; set; }
        public List<Word> items { get; set; } = new List<Word>();
    }
    public class Word
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? body { get; set; }
        public DateTime createAt { get; set; }
        public bool bookMark { get; set; } = false;
    }
}
