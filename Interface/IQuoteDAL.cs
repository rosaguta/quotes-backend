using System.Collections.Generic;
using DTO;
namespace Interface;

public interface IQuoteDAL
{
    QuoteDTO? GetRandomQuote(int randomint);
    bool NewQuote(QuoteDTO QuoteDTO);
    List<QuoteDTO> GetAllQuotes();
    int CountDocuments();
}