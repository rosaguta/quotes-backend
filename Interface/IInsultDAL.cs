using DTO;
namespace Interface;

public interface IInsultDAL
{
    QuoteDTO? GetRandomInsult(int randomint);
    bool NewInsult(QuoteDTOPost InsultDTO);
    List<QuoteDTO> GetAllInsults(bool HasRights);
    int CountDocuments();
    bool UpdateInsult(string id, QuoteDTO InsultDto, bool HasRights);
    bool DeleteInsult(string id);
}