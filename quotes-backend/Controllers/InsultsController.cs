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
    public IActionResult GetRandom(bool withContext, bool asObject)
    {
        object? quote;
        if(withContext){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                if(asObject){
                    quote = _insultsCollection.GetRandomInsult(true, true);
                }
                else
                {
                    quote =  _insultsCollection.GetRandomInsult(true, false);
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
                quote = _insultsCollection.GetRandomInsult(false, true);
            }
            else
            {
                quote =  _insultsCollection.GetRandomInsult(false, false);
            }
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
    public IActionResult AllInsults(string? text)
    {
        List<Quote> allInsults = new List<Quote>();
        if(text is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allInsults.Add(_insultsCollection.FindInsultBasedOnText(text));
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
                allInsults = _insultsCollection.GetAllInsults(true);
            }
            else
            {
                allInsults = _insultsCollection.GetAllInsults(true);
            }
        }
        if(allInsults.Count != 0){
            return Ok(allInsults);
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