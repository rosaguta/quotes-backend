using DTO;

namespace Logic.Mapper;

public static class MarkovDataMapper
{
    public static MarkovData ConvertToLogic(this MarkovDataDTO markovDataDto)
    {
        return new MarkovData
        {
            Text = markovDataDto.Text,
            DiscordMessageId = markovDataDto.DiscordMessageId,
            DiscordChannelId = markovDataDto.DiscordChannelId,
            DiscordGuildId = markovDataDto.DiscordGuildId,
            DiscordUserId = markovDataDto.DiscordUserId,
            DiscordUsername = markovDataDto.DiscordUsername,
            DiscordDiscriminator = markovDataDto.DiscordDiscriminator,
            ImagePath = markovDataDto.ImagePath
        };
    }

    public static MarkovDataDTO ConvertToDto(this MarkovData markovData)
    {
        return new MarkovDataDTO
        {
            Text = markovData.Text,
            DiscordMessageId = markovData.DiscordMessageId,
            DiscordChannelId = markovData.DiscordChannelId,
            DiscordGuildId = markovData.DiscordGuildId,
            DiscordUserId = markovData.DiscordUserId,
            DiscordUsername = markovData.DiscordUsername,
            DiscordDiscriminator = markovData.DiscordDiscriminator,
            ImagePath = markovData.ImagePath
        };
    }
}