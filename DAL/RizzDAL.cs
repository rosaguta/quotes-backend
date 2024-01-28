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
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("rizz");
        var totalCount = collection.CountDocuments(new BsonDocument());  
        int totalCountInt = (int)totalCount;
        return totalCountInt;
    }

    public bool NewRizz(QuoteDTOPost quoteDto)
    {
        BsonDocument bsonDocument = quoteDto.ToBsonDocument();

        try
        {
            IMongoCollection<BsonDocument> collection =
                _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("rizz");
            collection.InsertOne(bsonDocument);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return false;
        }
        
        return true;
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
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("rizz");
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

    public bool UpdateRizz(string id, QuoteDTO quoteDto)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("rizz");
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
        var update = Builders<BsonDocument>.Update
            .Set("Text", quoteDto.text)
            .Set("Person", quoteDto.person)
            .Set("DateTimeCreated", quoteDto.DateTimeCreated);
        var result = collection.UpdateOne(filter, update);
        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            return true;
        }

        return false;
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