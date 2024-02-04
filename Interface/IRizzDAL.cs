using DTO;

namespace Interface;

public interface IRizzDAL
{
    QuoteDTO? GetRandomRizz(int randomint);
    bool NewRizz(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllRizz();
    int CountDocuments();
    bool UpdateRizz(string id, QuoteDTO quoteDto);
    bool DeleteRizz(string id);
}