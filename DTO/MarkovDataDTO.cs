using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO;

public class MarkovDataDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    public required string Text { get; set; }
    public required string DiscordMessageId { get; set; }
    public required string DiscordChannelId { get; set; }
    public required string DiscordGuildId { get; set; }
    public required string DiscordUserId { get; set; }
    public required string DiscordUsername { get; set; }
    public required string DiscordDiscriminator { get; set; }
    public required string ImagePath { get; set; } // This is the image path from within the discord bot
}