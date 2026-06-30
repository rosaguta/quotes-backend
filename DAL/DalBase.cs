using MongoDB.Driver;

namespace DAL;

public abstract class DalBase
{
    protected readonly MongoClient MongoClient;

    protected DalBase()
    {
        try
        {
            MongoClient = new MongoClient(GetConnectionString());
        }
        catch
        {
            throw new Exception("connectionstring is incorrect");
        }
    }

    protected IMongoDatabase GetDatabase(string databaseName)
    {
        return MongoClient.GetDatabase(databaseName);
    }

    protected IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
    {
        return GetDatabase(databaseName).GetCollection<T>(collectionName);
    }

    private static string GetConnectionString()
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

        return $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}/";
    }
}
