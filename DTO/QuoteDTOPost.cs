using MongoDB.Bson.Serialization.Attributes;

namespace DTO;

public class QuoteDTOPost
{
    public string Text { get; set; }
    public string Person { get; set; }
    public string? Context { get; set; }
    public DateTime DateTimeCreated { get; set; }
    
}