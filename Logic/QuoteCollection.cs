﻿using DTO;
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

    public bool NewQuote(Quote quote)
    {
        QuoteDTO quoteDto = quote.ConvertToDTO();
        bool created =_QuoteInterface.NewQuote(quoteDto);
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
    private int GetLenghtOfDB()
    {
        int len = _QuoteInterface.GetAllQuotes().Count;
        return len;
    }
}