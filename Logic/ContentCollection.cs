using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DTO;
using Interface;
using Logic.Mapper;

namespace Logic;

public class ContentCollection
{
    private List<Quote> _items = new List<Quote>();
    private static List<string> RecentlyRequestedItems = new List<string>();
    private Random _random;
    private readonly IContentDAL _dal;
    private static Quote _lastItem = new Quote() { Context = "", DateTimeCreated = new DateTime(), person = "", text = "" };

    public ContentCollection(IContentDAL dal)
    {
        int seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
        _random = new Random(seed);
        _dal = dal;
    }

    public object? GetRandom(bool hasRights, bool asObject)
    {
        int lengthOfDB = GetLengthOfDB();
        if (lengthOfDB == 0) return null;
        
        int randomInt = _random.Next(0, lengthOfDB);
        QuoteDTO? dto;
        Quote item;

        int attempts = 0;
        do
        {
            dto = _dal.GetRandom(randomInt, hasRights);
            if (dto is null) return null;
            item = dto.ConvertToLogic();

            if (attempts < 10 && (BenjiCheck(item) || RecentlyAdded(item)))
            {
                randomInt = _random.Next(0, lengthOfDB);
                attempts++;
                continue;
            }
            break;
        } while (true);

        _lastItem = item;
        AddAndRemove(item);

        if (asObject) return item;
        return hasRights ? item.ToStringWithContext() : item.ToString();
    }

    private bool BenjiCheck(Quote item)
    {
        var comp = StringComparison.OrdinalIgnoreCase;
        return _lastItem.person.Contains("benj", comp) && item.person.Contains("benj", comp);
    }

    private bool RecentlyAdded(Quote item)
    {
        return RecentlyRequestedItems.Contains(item.ToString());
    }

    private void AddAndRemove(Quote item)
    {
        if (RecentlyRequestedItems.Count >= 15)
        {
            RecentlyRequestedItems.RemoveAt(RecentlyRequestedItems.Count - 1);
        }
        RecentlyRequestedItems.Insert(0, item.ToString());
    }

    public Quote GetById(string id)
    {
        return _dal.GetById(id).ConvertToLogic();
    }

    public Quote? New(QuoteDTOPost dto)
    {
        return _dal.New(dto).ConvertToLogic();
    }

    public List<Quote> GetAll(bool hasRights)
    {
        List<QuoteDTO> dtos = _dal.GetAll(hasRights);
        List<Quote> logicItems = new List<Quote>();
        foreach (var dto in dtos)
        {
            logicItems.Add(dto.ConvertToLogic());
        }
        return logicItems;
    }

    public Quote FindByText(string text)
    {
        return _dal.FindByText(text).ConvertToLogic();
    }

    public Quote FindByContext(string context)
    {
        return _dal.FindByContext(context).ConvertToLogic();
    }

    public bool Update(string id, Quote item)
    {
        return _dal.Update(id, item.ConvertToDTO());
    }

    public bool Delete(string id)
    {
        return _dal.Delete(id);
    }

    private int GetLengthOfDB()
    {
        // Some DALs use CountDocuments, others use GetAll().Count. 
        // Using CountDocuments as it's more efficient if implemented correctly.
        return _dal.CountDocuments();
    }
}
