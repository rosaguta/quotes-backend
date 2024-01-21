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
        _QuoteInterface = Factory.FactoryQuotaDal.Get();
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

    public List<Quote> GetAllQuotes()
    {
        List<QuoteDTO> allDTOquotes = _QuoteInterface.GetAllQuotes();
        foreach (QuoteDTO quoteDto in allDTOquotes)
        {
            Quotes.Add(quoteDto.ConvertToLogic());
        }

        return Quotes;
    }

    public bool UpdateQuote(string id, Quote quote)
    {
        QuoteDTO quoteDto = quote.ConvertToDTO();
        bool updated = _QuoteInterface.UpdateQuote(id, quoteDto);
        return updated;
    }
    private int GetLenghtOfDB()
    {
        int len = _QuoteInterface.GetAllQuotes().Count;
        return len;
    }
}