using DTO;
using Interface;

namespace DAL;

public class InsultDAL : ContentDalBase, IInsultDAL
{
    public InsultDAL() : base("insults")
    {
    }

    public QuoteDTO? GetRandomInsult(int randomint, bool hasRights) => GetRandom(randomint, hasRights);
    public QuoteDTO? NewInsult(QuoteDTOPost insultDto) => New(insultDto);
    public List<QuoteDTO> GetAllInsults(bool hasRights) => GetAll(hasRights);
    public bool UpdateInsult(string id, QuoteDTO insultDto) => Update(id, insultDto);
    public bool DeleteInsult(string id) => Delete(id);
    public QuoteDTO? FindInsultBasedOnText(string text) => FindByText(text);
    public QuoteDTO? FindInsultBasedOnContext(string context) => FindByContext(context);
}