using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DTO;
using Interface;
using Factory;
using Logic.Mapper;

namespace Logic;

public class QuoteCollection
{
    public List<Quote> Quotes { get; set; }
    
    readonly IQuoteDAL _QuoteInterface;
    private static readonly Random _random = new Random();
    private static int _NotBenjiCount = 0;

    public QuoteCollection()
    {
        Quotes = new List<Quote>();
        _QuoteInterface = DalFactory.GetQuoteDal();
    }

    public string? GetRandomQuote()
    {
        int lengthOfDB = GetLenghtOfDB();
        int randomInt = _random.Next(0, lengthOfDB);
    
        QuoteDTO? quoteDto = _QuoteInterface.GetRandomQuote(randomInt);
        if (quoteDto is null)
        {
            return null;
        }
    
        Quote quote = quoteDto.ConvertToLogic();
    
        
        if (BenjiCheck(quote) && _NotBenjiCount < 5)
        {
            _NotBenjiCount++;
            Try_ResetCount();
            return GetRandomQuote();
        }
    
        return quote.ToString();
    }


    private bool BenjiCheck(Quote q)
    {
        var comp = StringComparison.OrdinalIgnoreCase;
        if (q.person.Contains("benj", comp))
        {
            Console.WriteLine("true");
            return true;
        }
        else
        {
            Console.WriteLine("false");
            return false;
        }
    }

    private void Try_ResetCount()
    {
        if(_NotBenjiCount >= 5){
            _NotBenjiCount = 0;
        }
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