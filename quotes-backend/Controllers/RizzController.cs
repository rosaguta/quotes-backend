using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RizzController : ControllerBase
{
    private RizzCollection _RizzCollection;

    public RizzController()
    {
        _RizzCollection = new RizzCollection();
    }
    [SwaggerOperation(
        Summary = "Gets random Rizz (if there are any)",
        Description = "Requires AUTH for retrieving context"
    )]
    [HttpGet]
    [Route("/Rizzes/Random")]
    public IActionResult GetRandom(bool withContext, bool asObject)
    {
        // Check if either parameter requires permission
        if ((withContext || asObject) && (!User.Identity.IsAuthenticated || !User.HasClaim(c => c.Type == "Rights" && c.Value == "True")))
        {
            return Unauthorized("Access denied: please log in with appropriate permissions.");
        }

        // Fetch the quote based on the parameters
        object? quote = _RizzCollection.GetRandomRizz(withContext, asObject);

        // Validate the quote object
        if (quote == null || quote.ToString() == string.Empty)
        {
            return BadRequest("An error occurred while fetching the quote. Please contact support.");
        }

        return Ok(quote);
    }
    [SwaggerOperation(
        Summary = "Gets all Rizz"
    )]
    [HttpGet]
    [Route("/Rizzes")]
    public IActionResult GetAllRizz(string? text, string? context)
    {
        if (text is not null && context is not null)
        {
            return BadRequest("You can only provide the `text` OR the `context` params");
        }
        List<Quote>? allquotes = new List<Quote>();
        if(text is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allquotes.Add(_RizzCollection.FindRizzBasedOnText(text));
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
                allquotes.Add(_RizzCollection.FindRizzBasedOnContext(context));
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
            allquotes = _RizzCollection.GetAllRizz(true);
        }
        else
        {
            allquotes = _RizzCollection.GetAllRizz(false);
        }
        if(allquotes.Count != 0){
            return Ok(allquotes);
        }

        return NoContent();
    }

    
    [SwaggerOperation(
        Summary = "Adds a new Rizz",
        Description = "Requires AUTH"
    )]
    [HttpPost]
    [Route("/Rizzes"), Authorize]
    public IActionResult NewRizz([FromBody] QuoteDTOPost rizzpost)
    {
        bool created= _RizzCollection.NewRizz(rizzpost);
        if (created)
        {
            return Ok(created);
        }

        return BadRequest(created);
    }
    [SwaggerOperation(
        Summary = "Edits a Rizz",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("/Rizzes/{id}"), Authorize]
    public IActionResult UpdateRizz(string id, [FromBody] Quote rizz)
    {
        bool updated = _RizzCollection.UpdateRizz(id, rizz);
        if (updated)
        {
            return Ok(updated);
            
        }
        return BadRequest(updated);
    }

    [HttpDelete]
    [Route("/Rizzes/{id}")]
    [SwaggerOperation(Summary = "Deletes a single rizz", Description = "Requires AUTH")]
    public IActionResult DeleteRizz(string id)
    {
        bool deleted = _RizzCollection.DeleteRizz(id);
        if (deleted)
        {
            return Ok();
        }

        return BadRequest();
    }

}