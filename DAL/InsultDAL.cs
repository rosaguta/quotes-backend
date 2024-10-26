using DTO;
using Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class InsultDAL : IInsultDAL
{
    private MongoClient? _mongodbclient;

    public InsultDAL()
    {
        try
        {
            _mongodbclient = new MongoClient(GetConnectionString());
        }
        catch
        {
            throw new Exception("connectionstring is incorrect");
        }
    }   
    
    public QuoteDTO? GetRandomInsult(int randomint, bool hasRights)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
        var filter = Builders<BsonDocument>.Filter.Empty;
        var doc = collection.Find(filter).Skip(randomint).Limit(1).FirstOrDefault();
        try
        {
            string dateTimeString = doc["DateTimeCreated"].ToString();
            DateTime dateTimeCreated = DateTime.Parse(dateTimeString);
            string? context = null;
            if (hasRights)
            {
                try
                {
                    context = doc["Context"].ToString();
                }
                catch{}
            }
            return new QuoteDTO
            {
                text = doc["Text"].ToString(),
                person = doc["Person"].ToString(),
                DateTimeCreated = dateTimeCreated,
                Context = context
            };
        }
        catch
        {
            return null;
        }
    }

    public List<QuoteDTO> GetAllInsults(bool HasRights)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
        var filter = Builders<BsonDocument>.Filter.Empty;
        var sortDefinition = Builders<BsonDocument>.Sort.Ascending("DateTimeCreated");
        var documents = collection.Find(filter).Sort(sortDefinition).ToList();
        List<QuoteDTO> quoteDtos = new List<QuoteDTO>();
        foreach(var doc in documents)
        {
            string dateTimeString = doc["DateTimeCreated"].ToString();
            DateTime dateTimeCreated = DateTime.Parse(dateTimeString);
            string? context = null;
            if(HasRights){
                try
                {
                    context = doc["Context"].ToString();
                }
                catch
                {
                }
            }
            try
            {
                quoteDtos.Add(new QuoteDTO
                {
                    id = doc["_id"].ToString() ,text = doc["Text"].ToString(), person = doc["Person"].ToString(), DateTimeCreated = dateTimeCreated, Context = context
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

    public QuoteDTO FindInsultBasedOnText(string text)
    {
        IMongoCollection<QuoteDTO> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<QuoteDTO>("insults");
        var filter = Builders<QuoteDTO>.Filter.Eq(q => q.text, text);
        var doc = collection.Find(filter).FirstOrDefault();
        try
        {
            string dateTimeString = doc.DateTimeCreated.ToString();
            DateTime dateTimeCreated = DateTime.Parse(dateTimeString);
            string? context = null;
            return new QuoteDTO
            {
                id = doc.id,
                text = doc.text,
                person = doc.person,
                DateTimeCreated = dateTimeCreated,
                Context = doc.Context
            };
        }
        catch
        {
            return null;
        }
    }
    public bool NewInsult(QuoteDTOPost InsultDTO)
    {
        BsonDocument bsonDocument = InsultDTO.ToBsonDocument();
        try
        {
            IMongoCollection<BsonDocument> collection =
                _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
            collection.InsertOne(bsonDocument);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return false;
        }
        return true;
    }
    public bool UpdateInsult(string id, QuoteDTO quoteDto, bool HasRights)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
        if(!HasRights){
            var update = Builders<BsonDocument>.Update
                .Set("Text", quoteDto.text)
                .Set("Person", quoteDto.person)
                .Set("DateTimeCreated", quoteDto.DateTimeCreated);
            var result = collection.UpdateOne(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return true;
            }
        }
        else
        {
            var update = Builders<BsonDocument>.Update
                .Set("Text", quoteDto.text)
                .Set("Person", quoteDto.person)
                .Set("DateTimeCreated", quoteDto.DateTimeCreated)
                .Set("Context", quoteDto.Context);
            var result = collection.UpdateOne(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return true;
            }
        }
        return false;
    }
    public int CountDocuments()
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
        var filter = Builders<BsonDocument>.Filter.Empty;
        var count = collection.CountDocuments(filter);
        int totalCountInt = (int)count;
        return totalCountInt;
    }



    public bool DeleteInsult(string id)
    {
        IMongoCollection<BsonDocument> collection =
            _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("insults");
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
        var result = collection.DeleteOne(filter);
        return result.DeletedCount > 0;
    }

    private string? GetConnectionString()
    {
        string? mongoHost = Environment.GetEnvironmentVariable("MONGODB");
        string? mongoUsername = Environment.GetEnvironmentVariable("MONGODB_USERNAME");
        string? mongoPassword = Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
        string? mongoPort = Environment.GetEnvironmentVariable("MONGODB_PORT");
        if (mongoHost is null)
        {
            Console.WriteLine("You must provide the host in the following Env variable: MONGODB");
            Environment.Exit(0);
        }
        if (mongoUsername is null)
        {
            Console.WriteLine("You must provide the username in the following Env variable: MONGODB_USERNAME");
            Environment.Exit(0);
        }
        if (mongoPassword is null)
        {
            Console.WriteLine("You must provide the password in the following Env variable: MONGODB_PASSWORD");
            Environment.Exit(0);
        }

        if (mongoPort is null)
        {
            Console.WriteLine("You must provide the port in the following Env variable: MONGODB_PORT");
            Environment.Exit(0);
        }

        string connectionstring = $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}/";
        return connectionstring;
    }
}