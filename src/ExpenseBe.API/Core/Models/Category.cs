using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseBe.Core.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
    }
} 