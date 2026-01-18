using DTO;
using System.Collections.Generic;

namespace Interface;

public interface IContentDAL
{
    QuoteDTO? GetRandom(int randomint, bool hasRights);
    QuoteDTO? New(QuoteDTOPost dto);
    List<QuoteDTO> GetAll(bool hasRights);
    int CountDocuments();
    bool Update(string id, QuoteDTO dto);
    bool Delete(string id);
    QuoteDTO FindByText(string text);
    QuoteDTO GetById(string id);
    QuoteDTO FindByContext(string context);
}
