using DTO;

namespace Interface;

public interface IRizzDAL : IContentDAL
{
    QuoteDTO? GetRandomRizz(int randomint, bool withRights);
    QuoteDTO? NewRizz(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllRizz(bool HasRights);
    bool UpdateRizz(string id, QuoteDTO quoteDto);
    bool DeleteRizz(string id);
    QuoteDTO FindRizzBasedOnText(string text);
    QuoteDTO FindRizzBasedOnContext(string Context);

}