using DTO;
using Interface;
using MongoDB.Driver;

namespace DAL;

public class LonerDAL : ILonerDAL
{
    private MongoClient? _mongodbclient;

    public LonerDAL()
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
    
    public bool PostTime(LonerDTO lonerDto)
    {
        try
        {
            var database = _mongodbclient.GetDatabase("Loner");
            var collection = database.GetCollection<LonerDTO>("Users");
            collection.InsertOne(lonerDto);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
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