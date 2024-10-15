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
public class InsultsController : ControllerBase
{
    private InsultsCollection _insultsCollection;

    public InsultsController()
    {
        _insultsCollection = new InsultsCollection();
    }

    [SwaggerOperation(
        Summary = "Gets random Insult",
        Description = "Requires AUTH for retrieving context"
    )]
    [HttpGet]
    [Route("Random")]
    public IActionResult GetRandom(bool withContext)
    {
        string? quote;
        if(withContext){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                quote = _insultsCollection.GetRandomInsult(true);
            }
            else
            {
                return Unauthorized("Please kys or login to retrieve the context");
            }
        }
        else
        {
            quote = _insultsCollection.GetRandomInsult(false);
        }
        if (quote == "" | quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }
    
    [SwaggerOperation(
        Summary = "Gets all insults from database"
    )]
    [HttpGet]
    [Route("/Insults")]
    public IActionResult AllInsults(string? text, string? context)
    {
        if (text is not null && context is not null)
        {
            return BadRequest("You can only provide the `text` OR the `context` params");
        }
        List<Quote> allquotes = new List<Quote>();
        if(text is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allquotes.Add(_insultsCollection.FindInsultBasedOnContext(text));
                if (allquotes.Count != 0)
                {
                    return Ok(allquotes);
                }
                return NoContent();
            }
            return Unauthorized("Please kys or login to retrieve the context");
        }
        if(context is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allquotes.Add(_insultsCollection.FindInsultBasedOnContext(context));
                if (allquotes.Count != 0 && allquotes[0].text is not null)
                {
                    return Ok(allquotes);
                }
                return NoContent();
            }
            return Unauthorized("Please kys or login to retrieve the context");
        }
        if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
        {
            allquotes = _insultsCollection.GetAllInsults(true);
        }
        else
        {
            allquotes = _insultsCollection.GetAllInsults(false);
        }
        if(allquotes.Count != 0){
            return Ok(allquotes);
        }

        return NoContent();
    }
    [SwaggerOperation(
        Summary = "Adds a new Insult",
        Description = "Requires AUTH"
    )]
    [HttpPost(Name = "NewInsult"), Authorize]
    public IActionResult NewQuote([FromBody] QuoteDTOPost insult)
    {
       
        bool created = _insultsCollection.NewInsult(insult);
        if (created)
        {
            return Ok();
        }
        return BadRequest();
    }
    [SwaggerOperation(
        Summary = "Edits an Insult",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public IActionResult Update(string id, [FromBody] Quote quote)
    {
        bool updated = false;
        if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
        { 
            updated = _insultsCollection.UpdateQuote(id, quote, true);
        }else{
            updated = _insultsCollection.UpdateQuote(id, quote, false);
        }
        
        if (updated)
        {
            return Ok();
        }
        return BadRequest();
    }
    [SwaggerOperation(
        Summary = "Deletes an Insult",
        Description = "Requires AUTH"
    )]
    [HttpDelete]
    [Route("{id}"),Authorize]
    public IActionResult Delete(string id)
    {
        bool deleted = _insultsCollection.DeleteInsult(id);
        if (deleted)
        {
            return Ok();
        }

        return BadRequest();
    }
}