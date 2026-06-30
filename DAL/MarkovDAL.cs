using DTO;
using Interface;
using MongoDB.Driver;

namespace DAL;

public class MarkovDAL : DalBase, IMarkovDAL
{
    private const string DatabaseName = "Markov";
    private const string CollectionName = "Messages";

    private IMongoCollection<MarkovDataDTO> MarkovCollection =>
        GetCollection<MarkovDataDTO>(DatabaseName, CollectionName);

    public MarkovDataDTO? GetMarkovData(string id)
    {
        try
        {
            var filter = Builders<MarkovDataDTO>.Filter.Eq(markovData => markovData.DiscordMessageId, id);
            return MarkovCollection.Find(filter).FirstOrDefault();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }

    public bool SaveMarkovData(MarkovDataDTO markovData)
    {
        try
        {
            MarkovCollection.InsertOne(markovData);
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return false;
        }
    }

    public List<MarkovDataDTO> GetAllMarkovData()
    {
        try
        {
            return MarkovCollection
                .Find(Builders<MarkovDataDTO>.Filter.Empty)
                .ToList();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return new List<MarkovDataDTO>();
        }
    }
    
}