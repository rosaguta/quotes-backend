using DTO;

namespace Logic.Mapper;

public static class LonerMapper
{
    public static Loner ConvertToLogic(this LonerDTO lonerDto)
    {
        return new Loner()
        {
            DiscordUuid = lonerDto.DiscordUuid,
            MillisBeenAlone = lonerDto.MillisBeenAlone
        };
    }

    public static LonerDTO convertToDto(this Loner loner)
    {
        return new LonerDTO()
        {
            DiscordUuid = loner.DiscordUuid,
            MillisBeenAlone = loner.MillisBeenAlone
        };
    }
}