using System.Runtime.CompilerServices;
using DTO;

namespace Logic.Mapper;

public static class LonerMapper
{
    public static Loner ConvertToLogic(this LonerDTO lonerDto)
    {
        Loner loner = new Loner();
        loner.AloneInMillis = lonerDto.AloneInMillis;
        loner.DiscordUuid = lonerDto.DiscordUuid;
        loner.DiscordUsername = lonerDto.DiscordUsername;
        loner.DiscordDiscriminator = lonerDto.DiscordDiscriminator;
        loner.StartTimeAlone = lonerDto.StartTimeAlone;
        loner.EndTimeAlone = lonerDto.EndTimeAlone;
        loner.DiscordVoiceChannelId = lonerDto.DiscordVoiceChannelId;
        loner.DiscordVoiceChannelName = lonerDto.DiscordVoiceChannelName;
        return loner;
    }

    public static LonerDTO convertToDto(this Loner loner)
    {
        return new LonerDTO()
        {
            Id = loner.GetId(),
            DiscordUuid = loner.DiscordUuid,
            DiscordUsername = loner.DiscordUsername,
            DiscordDiscriminator = loner.DiscordDiscriminator,
            StartTimeAlone = loner.StartTimeAlone,
            EndTimeAlone = loner.EndTimeAlone,
            DiscordVoiceChannelId = loner.DiscordVoiceChannelId,
            DiscordVoiceChannelName = loner.DiscordVoiceChannelName,
            AloneInMillis = loner.GetAloneDurationInMillis()
        };
    }
}