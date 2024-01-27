using System.Collections.Generic;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class QuotesController : ControllerBase
{
    private QuoteCollection _quoteCollection;
    public QuotesController()
    {
        _quoteCollection = new QuoteCollection();
    }
    
    [HttpGet]
    [Route("Random")]
    public IActionResult GetRandom()
    {
        string? quote = new QuoteCollection().GetRandomQuote();
        if (quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }

    [HttpGet]
    public List<Quote> AllQuotes()
    {
        List<Quote> allquotes = _quoteCollection.GetAllQuotes();
        return allquotes;
    }
    [HttpPost(Name = "NewQuote")]
    public IActionResult NewQuote([FromBody] QuoteDTOPost quote)
    {
        _quoteCollection = new QuoteCollection();
        
        bool created = _quoteCollection.NewQuote(quote);
        if (created)
        {
            return Ok(created);
        }
        return BadRequest(created);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(string id, [FromBody] Quote quote)
    {
        _quoteCollection.Quotes.Add(quote);
        bool updated = _quoteCollection.UpdateQuote(id, quote);
        return Ok();
    }
    
}