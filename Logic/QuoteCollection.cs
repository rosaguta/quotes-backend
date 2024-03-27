using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    private static Quote _LastQuote = new Quote(){Context = "",DateTimeCreated = new DateTime(), person = "", text = ""};

    public QuoteCollection()
    {
        Quotes = new List<Quote>();
        _QuoteInterface = DalFactory.GetQuoteDal();
    }

    public string? GetRandomQuote()
    {
        int lengthOfDB = GetLenghtOfDB();
        int randomInt = _random.Next(0, lengthOfDB);
        QuoteDTO? quoteDto;
        Quote quote;
        var comp = StringComparison.OrdinalIgnoreCase;

        do
        {
            quoteDto = _QuoteInterface.GetRandomQuote(randomInt);
            if (quoteDto is null)
            {
                return null;
            }

            quote = quoteDto.ConvertToLogic();

            if (_LastQuote.person.Contains("benj", comp))
            {
                if (quote.person.Contains("benj", comp))
                {
                    // If both the current and last quote contain "benj", fetch a new random quote.
                    randomInt = _random.Next(0, lengthOfDB);
                    continue;
                }
            }

            // If the condition is met (either the last quote didn't contain "benj" or the current one doesn't), exit the loop.
            break;

        } while (true);

        _LastQuote = quote;
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