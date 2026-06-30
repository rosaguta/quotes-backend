using DTO;
using Interface;

namespace DAL;

public class RizzDAL : ContentDalBase, IRizzDAL
{
    public RizzDAL() : base("rizz")
    {
    }

    public QuoteDTO? GetRandomRizz(int randomint, bool withRights) => GetRandom(randomint, withRights);
    public QuoteDTO? NewRizz(QuoteDTOPost quoteDto) => New(quoteDto);
    public List<QuoteDTO> GetAllRizz(bool hasRights) => GetAll(hasRights);
    public bool UpdateRizz(string id, QuoteDTO quoteDto) => Update(id, quoteDto);
    public bool DeleteRizz(string id) => Delete(id);
    public QuoteDTO? FindRizzBasedOnText(string text) => FindByText(text);
    public QuoteDTO? FindRizzBasedOnContext(string context) => FindByContext(context);
}