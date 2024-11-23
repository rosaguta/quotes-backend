using System.Runtime.CompilerServices;
using DTO;

namespace Logic.Mapper;

public static class LonerMapper
{
    public static Loner ConvertToLogic(this LonerDTO lonerDto)
    {
        return new Loner(lonerDto.Id)
        {
            DiscordUuid = lonerDto.DiscordUuid,
            DiscordUsername = lonerDto.DiscordUsername,
            DiscordDiscriminator = lonerDto.DiscordDiscriminator,
            StartTimeAlone = lonerDto.StartTimeAlone,
            EndTimeAlone = lonerDto.EndTimeAlone,
            DiscordVoiceChannelId = lonerDto.DiscordVoiceChannelId,
            DiscordVoiceChannelName = lonerDto.DiscordVoiceChannelName,
        };
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