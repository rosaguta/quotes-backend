﻿using System;
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
    private List<Quote> Quotes { get; set; }
    private List<Quote> RecentlyRequestedQuotes { get; set; }
    
    readonly IQuoteDAL _QuoteInterface;
    private static readonly Random _random = new Random();
    private static Quote _LastQuote = new Quote(){Context = "",DateTimeCreated = new DateTime(), person = "", text = ""};

    public QuoteCollection()
    {
        Quotes = new List<Quote>();
        _QuoteInterface = DalFactory.GetQuoteDal();
        RecentlyRequestedQuotes = new List<Quote>();
    }

    public string? GetRandomQuote()
    {
        int lengthOfDB = GetLenghtOfDB();
        int randomInt = _random.Next(0, lengthOfDB);
        QuoteDTO? quoteDto;
        Quote quote;
        
        do
        {
            quoteDto = _QuoteInterface.GetRandomQuote(randomInt);
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
            else
            {
                
            }
            
            break;
        } while (true);
        _LastQuote = quote;
        RecentlyRequestedQuotes.Add(quote);
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
        if (RecentlyRequestedQuotes.Contains(quote))
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
            RecentlyRequestedQuotes.Insert(0,quote);
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