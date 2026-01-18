using DTO;
namespace Interface;

public interface IInsultDAL : IContentDAL
{
    QuoteDTO? GetRandomInsult(int randomint, bool hasRights);
    QuoteDTO? NewInsult(QuoteDTOPost InsultDTO);
    List<QuoteDTO> GetAllInsults(bool HasRights);
    bool UpdateInsult(string id, QuoteDTO InsultDto);
    bool DeleteInsult(string id);
    QuoteDTO FindInsultBasedOnText(string text);
    QuoteDTO FindInsultBasedOnContext(string Context);

}