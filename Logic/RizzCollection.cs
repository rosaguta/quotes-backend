using DTO;
using Interface;
using Factory;
using Logic.Mapper;

namespace Logic;

public class RizzCollection
{
    public List<Quote>? Rizzes { get; set; }

    IRizzDAL _rizzInterface;
    Random _random;

    public RizzCollection()
    {
        Rizzes = new List<Quote>();
        _rizzInterface = DalFactory.Getrizz();
    }
    public List<Quote>? GetAllRizz()
    {
        List<QuoteDTO> RizzDtos = _rizzInterface.GetAllRizz();
        foreach (QuoteDTO rizzdto in RizzDtos)
        {
            Rizzes.Add(rizzdto.ConvertToLogic()); 
        }

        if (Rizzes is not null)
        {
            return Rizzes;    
        }

        return null;
    }

    public string GetRandomRizz()
    {
        int lenghtofdb = GetLenghtOfDB();
        int randomint = _random.Next(0, lenghtofdb);
        
        QuoteDTO? rizzDto = _rizzInterface.GetRandomRizz(randomint);
        if (rizzDto is null)
        {
            return null;
        }
        Quote rizz = rizzDto.ConvertToLogic();
        return rizz.ToString();
    }

    public bool NewRizz(QuoteDTOPost rizzPost)
    {
        bool created = _rizzInterface.NewRizz(rizzPost);
        return created;
    }

    public bool UpdateRizz(string id, Quote rizz)
    {
        QuoteDTO quoteDto = rizz.ConvertToDTO();
        bool updated = _rizzInterface.UpdateRizz(id, quoteDto);
        return updated;
    }
    private int GetLenghtOfDB()
    {
        int len = _rizzInterface.GetAllRizz().Count;
        return len;
    }
}