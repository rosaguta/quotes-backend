using System.Security.Cryptography;
using DTO;
using Interface;
using Factory;
using Logic.Mapper;

namespace Logic;

public class RizzCollection
{
    public List<Quote>? Rizzes { get; set; }
    private Random _random { get; set; }

    IRizzDAL _rizzInterface;
    public RizzCollection()
    {
        int seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
        _random = new Random(seed);
        Rizzes = new List<Quote>();
        _rizzInterface = DalFactory.GetRizzDal();
    }
    public List<Quote>? GetAllRizz(bool HasRights)
    {
        List<QuoteDTO> RizzDtos = _rizzInterface.GetAllRizz(HasRights);
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

    public bool DeleteRizz(string id)
    {
        return _rizzInterface.DeleteRizz(id);
    }
    
    private int GetLenghtOfDB()
    {
        int len = _rizzInterface.GetAllRizz(false).Count;
        return len;
    }
}