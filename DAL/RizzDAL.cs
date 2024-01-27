using DTO;
using Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class RizzDAL : IRizzDAL
{
    private MongoClient _mongodbclient; 
    public RizzDAL()
    {
        _mongodbclient = new MongoClient(GetConnectionString());
    }   
    public int CountDocuments()
    {
        throw new NotImplementedException();
    }

    public bool NewRizz(QuoteDTOPost QuoteDTO)
    {
        throw new NotImplementedException();
    }

    public List<QuoteDTO> GetAllRizz()
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("rizz");
        var filter = Builders<BsonDocument>.Filter.Empty;
        var documents = collection.Find(filter).ToList();
        List<QuoteDTO> quoteDtos = new List<QuoteDTO>();
        foreach(var doc in documents)
        {
            string dateTimeString = doc["DateTimeCreated"].ToString();
            DateTime dateTimeCreated = DateTime.Parse(dateTimeString);
            try
            {
                quoteDtos.Add(new QuoteDTO
                {
                    id = doc["_id"].ToString() ,text = doc["Text"].ToString(), person = doc["Person"].ToString(), DateTimeCreated = dateTimeCreated
                });
            }
            catch
            {
                quoteDtos.Add(new QuoteDTO
                {
                    text = "LOL something went wrong in the backend, blame Rose :3", person = "admin", DateTimeCreated = DateTime.Now
                });
            }
        }

        return quoteDtos;
    }

    public QuoteDTO? GetRandomRizz(int randomint)
    {
        throw new NotImplementedException();
    }

    public bool UpdateRizz(string id, QuoteDTO quoteDto)
    {
        throw new NotImplementedException();
    }
    
    private string? GetConnectionString()
    {
        string? connectionstring = Environment.GetEnvironmentVariable("MONGODB");
        if (connectionstring is null)
        {
            Console.WriteLine("You must provide an Environmental variable: MONGODB");
            Environment.Exit(0);
        }
        return connectionstring;
    }
}