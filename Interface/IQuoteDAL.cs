using System.Collections.Generic;
using DTO;
namespace Interface;

public interface IQuoteDAL : IContentDAL
{
    QuoteDTO? GetRandomQuote(int randomint, bool hasRights);
    QuoteDTO? NewQuote(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllQuotes(bool HasRights);
    bool UpdateQuote(string id, QuoteDTO quoteDto);
    bool DeleteQuote(string id);
    QuoteDTO FindQuoteBasedOnText(string text);
    QuoteDTO GetQuote(string id);
    QuoteDTO FindQuoteBasedOnContext(string Context);
}