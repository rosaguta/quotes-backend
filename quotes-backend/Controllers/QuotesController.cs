using System.Collections.Generic;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Gets random Quote"
    )]
    [HttpGet]
    [Route("Random")]
    public IActionResult GetRandom()
    {
        string? quote = _quoteCollection.GetRandomQuote();
        if (quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }
    [SwaggerOperation(
        Summary = "Gets all quotes from database"
    )]
    [HttpGet]
    [Route("/Quotes")]
    public List<Quote> AllQuotes()
    {
        List<Quote> allquotes = _quoteCollection.GetAllQuotes();
        return allquotes;
    }
    [SwaggerOperation(
        Summary = "Adds a new Quote",
        Description = "Requires AUTH"
    )]
    [HttpPost(Name = "NewQuote"), Authorize]
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
    [SwaggerOperation(
        Summary = "Edits a quote",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public IActionResult Update(string id, [FromBody] Quote quote)
    {
        _quoteCollection.Quotes.Add(quote);
        bool updated = _quoteCollection.UpdateQuote(id, quote);
        if (updated)
        {
            return Ok();
        }

        return BadRequest();

    }
    [SwaggerOperation(
        Summary = "Deletes a Quote",
        Description = "Requires AUTH"
    )]
    [HttpDelete]
    [Route("{id}"),Authorize]
    public IActionResult Delete(string id)
    {
        bool deleted = _quoteCollection.DeleteQuote(id);
        if (deleted)
        {
            return Ok();
        }

        return BadRequest();
    }
    
}