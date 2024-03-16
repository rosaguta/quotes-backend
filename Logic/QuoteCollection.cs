using System;
using System.Collections.Generic;
using DTO;
using Interface;
using Factory;
using Logic.Mapper;

namespace Logic;

public class QuoteCollection
{
    public List<Quote> Quotes { get; set; }
    
    IQuoteDAL _QuoteInterface;
    Random _random;

    public QuoteCollection()
    {
        Quotes = new List<Quote>();
        _QuoteInterface = DalFactory.GetQuoteDal();
        _random = new Random();
    }
    public string? GetRandomQuote()
    {
        int lenghtofdb = GetLenghtOfDB();
        int randomint = _random.Next(0, lenghtofdb);
        
        QuoteDTO? quoteDto = _QuoteInterface.GetRandomQuote(randomint);
        if (quoteDto is null)
        {
            return null;
        }
        Quote quote = quoteDto.ConvertToLogic();
        return quote.ToString();
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