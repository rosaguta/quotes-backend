using System.Collections.Generic;
using DTO;
namespace Interface;

public interface IQuoteDAL
{
    QuoteDTO? GetRandomQuote(int randomint, bool hasRights);
    bool NewQuote(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllQuotes(bool HasRights);
    int CountDocuments();
    bool UpdateQuote(string id, QuoteDTO quoteDto, bool HasRights);
    bool DeleteQuote(string id);
}