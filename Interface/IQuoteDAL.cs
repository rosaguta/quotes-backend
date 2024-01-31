﻿using System.Collections.Generic;
using DTO;
namespace Interface;

public interface IQuoteDAL
{
    QuoteDTO? GetRandomQuote(int randomint);
    bool NewQuote(QuoteDTOPost QuoteDTO);
    List<QuoteDTO> GetAllQuotes();
    int CountDocuments();
    bool UpdateQuote(string id, QuoteDTO quoteDto);
    bool DeleteQuote(string id);
}