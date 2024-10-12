using DTO;

namespace Interface;

public interface IRizzDAL
{
    QuoteDTO? GetRandomRizz(int randomint, bool withRights);
    bool NewRizz(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllRizz(bool HasRights);
    int CountDocuments();
    bool UpdateRizz(string id, QuoteDTO quoteDto);
    bool DeleteRizz(string id);
}