using MongoDB.Bson.Serialization.Attributes;

namespace DTO;

public class QuoteDTOPost
{
    [BsonElement("Text")]
    public string text { get; set; }
    [BsonElement("Person")]
    public string person { get; set; }
    [BsonElement("DateTimeCreated")]
    public DateTime DateTimeCreated { get; set; }
}