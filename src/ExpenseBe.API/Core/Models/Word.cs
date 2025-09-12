using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ExpenseBe.Core.Models
{
    public class Word
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? body { get; set; }
        public DateTime createAt { get; set; }
    }
}
