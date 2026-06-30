using DTO;
using Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL;

public abstract class ContentDalBase : DalBase, IContentDAL
{
    private const string DatabaseName = "Quotes";

    private readonly string _collectionName;

    protected ContentDalBase(string collectionName)
    {
        _collectionName = collectionName;
    }

    protected IMongoCollection<BsonDocument> BsonCollection =>
        GetCollection<BsonDocument>(DatabaseName, _collectionName);

    protected IMongoCollection<QuoteDTO> QuoteCollection =>
        GetCollection<QuoteDTO>(DatabaseName, _collectionName);

    public QuoteDTO? GetRandom(int randomint, bool hasRights)
    {
        var doc = BsonCollection
            .Find(Builders<BsonDocument>.Filter.Empty)
            .Skip(randomint)
            .Limit(1)
            .FirstOrDefault();

        return MapDocumentToQuoteDto(doc, hasRights, includeId: false);
    }

    public QuoteDTO? New(QuoteDTOPost dto)
    {
        BsonDocument bsonDocument = dto.ToBsonDocument();

        try
        {
            BsonCollection.InsertOne(bsonDocument);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }

        return MapDocumentToQuoteDto(bsonDocument, hasRights: true, includeId: true);
    }

    public List<QuoteDTO> GetAll(bool hasRights)
    {
        var sortDefinition = Builders<BsonDocument>.Sort.Ascending("DateTimeCreated");

        var documents = BsonCollection
            .Find(Builders<BsonDocument>.Filter.Empty)
            .Sort(sortDefinition)
            .ToList();

        List<QuoteDTO> quoteDtos = new();

        foreach (var doc in documents)
        {
            QuoteDTO? quoteDto = MapDocumentToQuoteDto(doc, hasRights, includeId: true);

            quoteDtos.Add(quoteDto ?? new QuoteDTO
            {
                text = "LOL something went wrong in the backend, blame Rose :3",
                person = "admin",
                DateTimeCreated = DateTime.Now
            });
        }

        return quoteDtos;
    }

    public int CountDocuments()
    {
        long totalCount = BsonCollection.CountDocuments(Builders<BsonDocument>.Filter.Empty);
        return (int)totalCount;
    }

    public bool Update(string id, QuoteDTO dto)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));

        var update = Builders<BsonDocument>.Update
            .Set("Text", dto.text)
            .Set("Person", dto.person)
            .Set("DateTimeCreated", dto.DateTimeCreated)
            .Set("Context", dto.Context);

        var result = BsonCollection.UpdateOne(filter, update);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public bool Delete(string id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
        var result = BsonCollection.DeleteOne(filter);

        return result.DeletedCount > 0;
    }

    public QuoteDTO? FindByText(string text)
    {
        var filter = Builders<QuoteDTO>.Filter.Eq(q => q.text, text);
        var doc = QuoteCollection.Find(filter).FirstOrDefault();

        return MapQuoteDto(doc);
    }

    public QuoteDTO? GetById(string id)
    {
        var filter = Builders<QuoteDTO>.Filter.Eq(q => q.id, id);
        return QuoteCollection.Find(filter).FirstOrDefault();
    }

    public QuoteDTO? FindByContext(string context)
    {
        var filter = Builders<QuoteDTO>.Filter.Eq(q => q.Context, context);
        var doc = QuoteCollection.Find(filter).FirstOrDefault();

        return MapQuoteDto(doc);
    }

    private static QuoteDTO? MapDocumentToQuoteDto(BsonDocument? doc, bool hasRights, bool includeId)
    {
        if (doc is null)
        {
            return null;
        }

        try
        {
            string? context = null;

            if (hasRights && doc.Contains("Context"))
            {
                context = doc["Context"].ToString();
            }

            QuoteDTO quoteDto = new()
            {
                text = doc["Text"].ToString(),
                person = doc["Person"].ToString(),
                DateTimeCreated = DateTime.Parse(doc["DateTimeCreated"].ToString()),
                Context = context
            };

            if (includeId && doc.Contains("_id"))
            {
                quoteDto.id = doc["_id"].ToString();
            }

            return quoteDto;
        }
        catch
        {
            return null;
        }
    }

    private static QuoteDTO? MapQuoteDto(QuoteDTO? doc)
    {
        if (doc is null)
        {
            return null;
        }

        try
        {
            return new QuoteDTO
            {
                id = doc.id,
                text = doc.text,
                person = doc.person,
                DateTimeCreated = DateTime.Parse(doc.DateTimeCreated.ToString()),
                Context = doc.Context
            };
        }
        catch
        {
            return null;
        }
    }
}