using System.Security.Cryptography;
using DTO;
using Factory;
using Interface;
using Logic.Mapper;

namespace Logic;

public class InsultsCollection
{
    public List<Quote> Insults { get; set; }
    private Random _random { get; set; }

    readonly IInsultDAL _InsultInterface;
    private static Quote _LastQuote = new Quote(){Context = "",DateTimeCreated = new DateTime(), person = "", text = ""};
    
    public InsultsCollection()
    {
        int seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
        _random = new Random(seed);
        Insults = new List<Quote>();
        _InsultInterface = DalFactory.GetInsultDal();
    }
    
    public string? GetRandomInsult()
    {
        int lengthOfDB = GetLenghtOfDB();
        int randomInt = _random.Next(0, lengthOfDB);
            QuoteDTO quoteDto = _InsultInterface.GetRandomInsult(randomInt);
            if (quoteDto is null)
            {
                return null;
            }
            Quote quote = quoteDto.ConvertToLogic();

        return quote.ToString();
    }

    public List<Quote> GetAllInsults(bool rights)
    {
        List<QuoteDTO> quoteDtos = _InsultInterface.GetAllInsults(rights);

        List<Quote> quotes = new List<Quote>();
        foreach (QuoteDTO quoteDto in quoteDtos)
        {
            quotes.Add(quoteDto.ConvertToLogic());
        }
        return quotes;
    }
    public bool NewInsult(QuoteDTOPost quote)
    {
        bool created =_InsultInterface.NewInsult(quote);
        return created;
    }
    public bool UpdateQuote(string id, Quote quote, bool HasRights)
    {
        QuoteDTO quoteDto = quote.ConvertToDTO();
        bool updated = _InsultInterface.UpdateInsult(id, quoteDto, HasRights);
        return updated;
    }
    public bool DeleteInsult(string id)
    {
        return _InsultInterface.DeleteInsult(id);
    }
    private int GetLenghtOfDB()
    {
        int len = _InsultInterface.CountDocuments();
        return len;
    }
    
}