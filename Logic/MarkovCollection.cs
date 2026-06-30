using DTO;
using Interface;
using Logic.Mapper;
namespace Logic;

public class MarkovCollection
{
    private readonly IMarkovDAL _markovDal;

    public MarkovCollection() : this(Factory.DalFactory.GetMarkovDal())
    {
    }

    public MarkovCollection(IMarkovDAL markovDal)
    {
        _markovDal = markovDal;
    }

    public List<MarkovData> GetAllMarkovData()
    {
        List<MarkovDataDTO> markovDataDtos = _markovDal.GetAllMarkovData();
        List<MarkovData> markovData = new List<MarkovData>();
        foreach (MarkovDataDTO markovDataDto in markovDataDtos)
        {
            markovData.Add(markovDataDto.ConvertToLogic());
        }

        return markovData;
    }

    public MarkovData? GetMarkovData(string discordMessageId)
    {
        MarkovDataDTO? markovDataDto = _markovDal.GetMarkovData(discordMessageId);

        if (markovDataDto is null)
        {
            return null;
        }

        return markovDataDto.ConvertToLogic();
    }

    public bool SaveMarkovData(MarkovData markovData)
    {
        MarkovDataDTO markovDataDto = markovData.ConvertToDto();
        return _markovDal.SaveMarkovData(markovDataDto);
    }

}