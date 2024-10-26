using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DTO;
using Interface;
using Factory;
using Logic.Mapper;

namespace Logic;

public class QuoteCollection
{
    private List<Quote> Quotes { get; set; }
    private static List<string> RecentlyRequestedQuotes = new List<string>();
    private Random _random { get; set; }
    readonly IQuoteDAL _QuoteInterface;
    private static Quote _LastQuote = new Quote(){Context = "",DateTimeCreated = new DateTime(), person = "", text = ""};

    public QuoteCollection()
    {
        int seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
        _random = new Random(seed);
        Quotes = new List<Quote>();
        _QuoteInterface = DalFactory.GetQuoteDal();
    }

    public object? GetRandomQuote(bool hasRights, bool asObject)
    {
        int lengthOfDB = GetLenghtOfDB();
        int randomInt = _random.Next(0, lengthOfDB);
        QuoteDTO? quoteDto;
        Quote quote;
        
        do
        {
            quoteDto = _QuoteInterface.GetRandomQuote(randomInt, hasRights);
            
            if (quoteDto is null)
            {
                return null;
            }
            quote = quoteDto.ConvertToLogic();

            if (BenjiCheck(quote))
            {
                randomInt = _random.Next(0, lengthOfDB);
                continue;
            }
            
            if (RecentlyAdded(quote))
            {
                randomInt = _random.Next(0, lengthOfDB);
                continue;
            }
            break;
        } while (true);
        _LastQuote = quote;
        AddAndRemoveQuote(quote);
        if(hasRights){
            if (asObject)
            {
                return quote;
            }
            return quote.ToStringWithContext();
        }

        if (asObject)
        {
            return quote;
        }
        return quote.ToString();
    }

    private bool BenjiCheck(Quote quote)
    {
        var comp = StringComparison.OrdinalIgnoreCase;
        if (_LastQuote.person.Contains("benj", comp))
        {
            if (quote.person.Contains("benj", comp))
            {
                return true;
            }
        }
        return false;
    }

    private bool RecentlyAdded(Quote quote)
    {
        if (RecentlyRequestedQuotes.Contains(quote.ToString()))
        {
            return true;
        }

        return false;
    }

    private void AddAndRemoveQuote(Quote quote)
    {
        int lenghtoflist = RecentlyRequestedQuotes.Count;
        if (lenghtoflist == 15)
        {
            RecentlyRequestedQuotes.RemoveAt(14);
        }
        if (lenghtoflist < 15)
        {
            RecentlyRequestedQuotes.Insert(0,quote.ToString());
        }
    }

    public Quote getQuote(string id)
    {
        QuoteDTO quoteDto = _QuoteInterface.GetQuote(id);
        Quote quote = quoteDto.ConvertToLogic();
        return quote;
    }
    public bool NewQuote(QuoteDTOPost quote)
    {
        bool created =_QuoteInterface.NewQuote(quote);
        return created;
    }
    public List<Quote> GetAllQuotes(bool HasRights)
    {
        List<QuoteDTO> allDTOquotes = _QuoteInterface.GetAllQuotes(HasRights);
        foreach (QuoteDTO quoteDto in allDTOquotes)
        {
            Quotes.Add(quoteDto.ConvertToLogic());
        }

        
        return Quotes;
    }

    public Quote FindQuoteBasedOnText(string text)
    {
        QuoteDTO? quoteDTO = _QuoteInterface.FindQuoteBasedOnText(text);
        Quote quote = quoteDTO.ConvertToLogic();
        return quote;
    }

    public Quote FindQuoteBasedOnContext(string context)
    {
        QuoteDTO? quoteDto = _QuoteInterface.FindQuoteBasedOnContext(context);
        return quoteDto.ConvertToLogic();
    }
    public bool UpdateQuote(string id, Quote quote, bool HasRights)
    {
        QuoteDTO quoteDto = quote.ConvertToDTO();
        bool updated = _QuoteInterface.UpdateQuote(id, quoteDto, HasRights);
        return updated;
    }

    public bool DeleteQuote(string id)
    {
        return _QuoteInterface.DeleteQuote(id);
    }
    private int GetLenghtOfDB()
    {
        int len = _QuoteInterface.GetAllQuotes(false).Count;
        return len;
    }
}