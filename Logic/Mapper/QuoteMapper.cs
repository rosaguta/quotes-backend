using DTO;

namespace Logic.Mapper;

public static class QuoteMapper
{
    public static Quote ConvertToLogic(this QuoteDTO quoteDto)
    {
        if (quoteDto is null)
        {
            return new Quote();
        }

        return new Quote()
        {
            id = quoteDto.id,
            text = quoteDto.text,
            person = quoteDto.person,
            DateTimeCreated = quoteDto.DateTimeCreated
        };
    }

    public static QuoteDTO ConvertToDTO(this Quote quote)
    {
        if (quote is null)
        {
            return new QuoteDTO();
        }

        return new QuoteDTO()
        {
            id = quote.id,
            text = quote.text,
            person = quote.person,
            DateTimeCreated = quote.DateTimeCreated
        };
    }
}