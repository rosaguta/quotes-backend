using DTO;
using Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public class UserDAL : IUserDAL
{
    private MongoClient _mongodbclient; 
    public UserDAL()
    {
        _mongodbclient = new MongoClient(GetConnectionString());
    }
    
    public UserDTO? GetUser(string username)
    {
        IMongoCollection<BsonDocument> collection = _mongodbclient.GetDatabase("Quotes").GetCollection<BsonDocument>("users");
        var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
        var result = collection.Find(filter).FirstOrDefault();

        if (result != null)
        {
            // The document with the specified username was found
            // Console.WriteLine(result.ToJson());
            return new UserDTO
            {
                Username = result["Username"].ToString(),
                Password = result["Password"].ToString(),
                Rights = result["Rights"].ToBoolean()
            };
        }
        else
        {
            // No document found with the specified username
            Console.WriteLine("User not found.");
            return null;
        }
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