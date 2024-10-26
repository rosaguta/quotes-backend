using System.Collections.Generic;
using System.Security.Claims;
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
        Summary = "Gets random Quote",
        Description = "Requires AUTH for retrieving context"
    )]
    [HttpGet]
    [Route("Random")]
    public IActionResult GetRandom(bool withContext, bool asObject)
    {
        object? quote;
        if(withContext){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                if(asObject){
                    quote = _quoteCollection.GetRandomQuote(true, true);
                }
                else
                {
                    quote = _quoteCollection.GetRandomQuote(true, false);
                }
            }
            else
            {
                return Unauthorized("Please kys or login to retrieve the context");
            }
        }
        else
        {
            if(asObject){
                quote = _quoteCollection.GetRandomQuote(false, true);
            }
            else
            {
                quote = _quoteCollection.GetRandomQuote(false, false);
            }
        }
        if (quote == "" | quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }

    [SwaggerOperation(
        Summary = "Gets all quotes from database",
        Description = "When the text field is filled in, the API will try to find the requested quote. Only works with AUTH"
    )]
    [HttpGet]
    [Route("/Quotes")]
    public IActionResult AllQuotes(string? text)
    {
        List<Quote> allquotes = new List<Quote>();
        if(text is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allquotes.Add(_quoteCollection.FindQuoteBasedOnText(text));
            }
            else
            {
                return Unauthorized("Please kys or login to retrieve the context");
            }
            
        }
        else
        {
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allquotes = _quoteCollection.GetAllQuotes(true);
            }
            else
            {
                allquotes = _quoteCollection.GetAllQuotes(false);
            }
        }
        if(allquotes.Count != 0){
            return Ok(allquotes);
        }

        return NoContent();
    }
    [SwaggerOperation(
        Summary = "Adds a new Quote",
        Description = "Requires AUTH"
    )]
    [HttpPost(Name = "NewQuote"), Authorize]
    public IActionResult NewQuote([FromBody] QuoteDTOPost quote)
    {
       
        bool created = _quoteCollection.NewQuote(quote);
        if (created)
        {
            return Ok();
        }
        return BadRequest();
    }
    [SwaggerOperation(
        Summary = "Edits a quote",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public IActionResult Update(string id, [FromBody] Quote quote)
    {
        bool updated = false;
        if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
        { 
            updated = _quoteCollection.UpdateQuote(id, quote, true);
        }else{
            updated = _quoteCollection.UpdateQuote(id, quote, false);
        }
        
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