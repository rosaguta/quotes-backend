using System.Collections.Generic;
using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class QuotesController : ControllerBase
{
    private QuoteCollection _quoteCollection;
    public QuotesController()
    {
        _quoteCollection = new QuoteCollection();
    }
    [SwaggerOperation(
        Summary = "Gets random Quote (requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("Random")]
    public IActionResult GetRandom(bool withContext, bool asObject)
    {
        // Check if either parameter requires permission
        if ((withContext) && (!Rights.hasRights(User)))
        {
            return Unauthorized("Access denied: please log in with appropriate permissions.");
        }

        // Fetch the quote based on the parameters
        object? quote = _quoteCollection.GetRandomQuote(withContext, asObject);

        // Validate the quote object
        if (quote == null || quote.ToString() == string.Empty)
        {
            return BadRequest("An error occurred while fetching the quote. Please contact support.");
        }

        return Ok(quote);
    }

    [SwaggerOperation(
        Summary = "Gets all quotes from database (requires AUTH)",
        Description = "When the text field is filled in, the API will try to find the requested quote. Only works with the correct rights"
    )]
    [HttpGet]
    [Route("/Quotes")]
    public IActionResult AllQuotes(string? text, string? context)
    {
        if (text is not null && context is not null)
        {
            return BadRequest("You can only provide the `text` OR the `context` params");
        }
        List<Quote> allquotes = new List<Quote>();
        if(text is not null){
            if (Rights.hasRights(User))
            {
                allquotes.Add(_quoteCollection.FindQuoteBasedOnText(text));
                if (allquotes.Count != 0)
                {
                    return Ok(allquotes);
                }
                return NoContent();
            }
            return Unauthorized("Please kys or login to retrieve the context");
        }
        if(context is not null){
            if (Rights.hasRights(User))
            {
                allquotes.Add(_quoteCollection.FindQuoteBasedOnContext(context));
                if (allquotes.Count != 0 && allquotes[0].text is not null)
                {
                    return Ok(allquotes);
                }
                return NoContent();
            }
            return Unauthorized("Please kys or login to retrieve the context");
        }
        if (Rights.hasRights(User))
        {
           allquotes = _quoteCollection.GetAllQuotes(true);
        }
        else
        {
            allquotes = _quoteCollection.GetAllQuotes(false);
        }
        if(allquotes.Count != 0){
            return Ok(allquotes);
        }

        return NoContent();
    }
    [SwaggerOperation(
        Summary = "Adds a new Quote (requires AUTH)",
        Description = "Requires special permissions to create a new quote"
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
        Summary = "Edits a quote (requires AUTH)",
        Description = "Requires special permissions to update"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public IActionResult Update(string id, [FromBody] Quote quote)
    {
        bool updated = false;
        if (Rights.hasRights(User))
        { 
            updated = _quoteCollection.UpdateQuote(id, quote);
        }
        
        if (updated)
        {
            return Ok();
        }

        return Forbid();

    }
    [SwaggerOperation(
        Summary = "Deletes a Quote (requires AUTH)",
    Description = "Requires special permissions to delete"
    )]
    [HttpDelete]
    [Route("{id}"),Authorize]
    public IActionResult Delete(string id)
    {
        if (Rights.hasRights(User)){
            bool deleted = _quoteCollection.DeleteQuote(id);
            if (deleted)
            {
                return Ok();
            }
        }
        return Forbid();
    }
}