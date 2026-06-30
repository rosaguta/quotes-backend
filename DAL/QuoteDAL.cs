using DTO;
using Interface;

namespace DAL;

public class QuoteDAL : ContentDalBase, IQuoteDAL
{
    public QuoteDAL() : base("quotes")
    {
    }

    public QuoteDTO? GetRandomQuote(int randomint, bool hasRights) => GetRandom(randomint, hasRights);
    public QuoteDTO? NewQuote(QuoteDTOPost quoteDto) => New(quoteDto);
    public List<QuoteDTO> GetAllQuotes(bool hasRights) => GetAll(hasRights);
    public bool UpdateQuote(string id, QuoteDTO quoteDto) => Update(id, quoteDto);
    public bool DeleteQuote(string id) => Delete(id);
    public QuoteDTO? FindQuoteBasedOnText(string text) => FindByText(text);
    public QuoteDTO? GetQuote(string id) => GetById(id);
    public QuoteDTO? FindQuoteBasedOnContext(string context) => FindByContext(context);
}