using System.Reflection.Metadata;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTO;

public class LonerDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("DiscordUuid")]
    public string DiscordUuid { get; set; }
    [BsonElement("DiscordUsername")]
    public string DiscordUsername { get; set; }
    [BsonElement("DiscordDiscriminator")]
    public string DiscordDiscriminator { get; set; }

    [BsonElement("StartTimeAlone")]
    public DateTime StartTimeAlone { get; set; }

    [BsonElement("EndTimeAlone")]
    public DateTime EndTimeAlone { get; set; }
    [BsonElement("DiscordVoiceChannelId")]
    public string DiscordVoiceChannelId { get; set; }

    [BsonElement("DiscordVoiceChannelName")]
    public string DiscordVoiceChannelName { get; set; }

    [BsonElement("AloneInMillis")]
    public long AloneInMillis { get; set; }
}