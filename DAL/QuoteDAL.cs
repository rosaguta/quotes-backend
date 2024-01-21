using DTO;
using Interface;

using MongoDB.Driver;
using MongoDB.Bson;

namespace DAL;

public class QuoteDAL : IQuoteDAL
{
    private MongoClient _mongodbclient;

    public QuoteDAL()
    {
        _mongodbclient = new MongoClient(GetConnectionString());
    }   
    public QuoteDTO GetRandomQuote(int randomint)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("quotes");
        var filter = Builders<BsonDocument>.Filter.Empty;
        var doc = collection.Find(filter).Skip(randomint).Limit(1).FirstOrDefault();
        try
        {
            string dateTimeString = doc["DateTimeCreated"].ToString();
            DateTime dateTimeCreated = DateTime.Parse(dateTimeString);

            return new QuoteDTO
            {
                text = doc["Text"].ToString(),
                person = doc["Person"].ToString(),
                DateTimeCreated = dateTimeCreated
            };
        }
        catch
        {
            return null;
        }
    }

    public bool NewQuote(QuoteDTO quoteDto)
    {
        BsonDocument bsonDocument = quoteDto.ToBsonDocument();

        try
        {
            IMongoCollection<BsonDocument> collection =
                _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("quotes");
            collection.InsertOne(bsonDocument);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return false;
        }
        
        return true;
    }

    public List<QuoteDTO> GetAllQuotes()
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("quotes");
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
                    text = doc["Text"].ToString(), person = doc["Person"].ToString(), DateTimeCreated = dateTimeCreated
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

    public int CountDocuments()
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("quotes");
        var totalCount = collection.CountDocuments(new BsonDocument());  
        int totalCountInt = (int)totalCount;
        return totalCountInt;
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