using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO{
public class QuoteDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    [BsonElement("Text")]
    public string text { get; set; }
    [BsonElement("Person")]
    public string person { get; set; }
    [BsonElement("Context")]
    public string? Context { get; set; }
    [BsonElement("DateTimeCreated")]
    public DateTime DateTimeCreated { get; set; }
    
    
}
}