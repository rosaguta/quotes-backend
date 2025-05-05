using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RizzController : ControllerBase
{
    private RizzCollection _RizzCollection;

    public RizzController()
    {
        _RizzCollection = new RizzCollection();
    }

    [SwaggerOperation(
        Summary = "Gets random Rizz (if there are any) (Requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("/Rizzes/Random")]
    public IActionResult GetRandom(bool withContext, bool asObject)
    {
        // Check if either parameter requires permission
        if ((withContext) && !Rights.hasRights(User))
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
        Summary = "Gets all Rizz (Requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
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
        if (text is not null)
        {
            if (Rights.hasRights(User))
            {
                allquotes.Add(_RizzCollection.FindRizzBasedOnText(text));
                if (allquotes.Count != 0)
                {
                    return Ok(allquotes);
                }

                return NoContent();
            }

            return Unauthorized("Login to retrieve the context");
        }

        if (context is not null)
        {
            if (Rights.hasRights(User))
            {
                allquotes.Add(_RizzCollection.FindRizzBasedOnContext(context));
                if (allquotes.Count != 0 && allquotes[0].text is not null)
                {
                    return Ok(allquotes);
                }

                return NoContent();
            }

            return Unauthorized("Login to retrieve the context");
        }

        if (Rights.hasRights(User))
        {
            allquotes = _RizzCollection.GetAllRizz(true);
        }
        else
        {
            allquotes = _RizzCollection.GetAllRizz(false);
        }

        if (allquotes.Count != 0)
        {
            return Ok(allquotes);
        }

        return NoContent();
    }


    [SwaggerOperation(
        Summary = "Adds a new Rizz (Requires AUTH)",
        Description = "Requires special permissions to create some new rizz"
    )]
    [HttpPost]
    [Route("/Rizzes"), Authorize]
    public IActionResult NewRizz([FromBody] QuoteDTOPost rizzpost)
    {
        if (Rights.hasRights(User))
        {
            bool created = _RizzCollection.NewRizz(rizzpost);
            if (created)
            {
                return Ok(created);
            }

            return BadRequest(created);
        }

        return Forbid();
    }

    [SwaggerOperation(
        Summary = "Edits a Rizz (Requires AUTH)",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("/Rizzes/{id}"), Authorize]
    public IActionResult UpdateRizz(string id, [FromBody] Quote rizz)
    {
        if (Rights.hasRights(User))
        {
            bool updated = _RizzCollection.UpdateRizz(id, rizz);
            if (updated)
            {
                return Ok(updated);
            }

            return BadRequest(updated);
        }

        return Forbid();
    }

    [HttpDelete]
    [Route("/Rizzes/{id}")]
    [SwaggerOperation(Summary = "Deletes a single rizz Requires AUTH",
        Description = "Requires special permissions to delete some rizz")]
    public IActionResult DeleteRizz(string id)
    {
        if (Rights.hasRights(User))
        {
            bool deleted = _RizzCollection.DeleteRizz(id);
            if (deleted)
            {
                return Ok();
            }

            return BadRequest();
        }

        return Forbid();
    }
}