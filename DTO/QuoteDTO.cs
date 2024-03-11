using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO{
public class QuoteDTO
{
    public string? id { get; set; }
    public string text { get; set; }
    public string person { get; set; }
    public string? Context { get; set; }
    public DateTime DateTimeCreated { get; set; }
    
    
}
}