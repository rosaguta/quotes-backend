using DTO;

namespace Interface;

public interface IMarkovDAL
{
    public MarkovDataDTO? GetMarkovData(string id);
    public bool SaveMarkovData(MarkovDataDTO markovData);
    public List<MarkovDataDTO> GetAllMarkovData();
}