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
    public IActionResult GetRandom(bool withContext)
    {
        string? quote;
        if(withContext){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                quote = _RizzCollection.GetRandomRizz(true);
            }
            else
            {
                return Unauthorized("Please kys or login to retrieve the context");
            }
        }
        else
        {
            quote = _RizzCollection.GetRandomRizz(false);
        }
        if (quote == "" | quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }
    [SwaggerOperation(
        Summary = "Gets all Rizz"
    )]
    [HttpGet]
    [Route("/Rizzes")]
    public IActionResult GetAllRizz(string? text)
    {
        List<Quote>? allRizz = new List<Quote>();
        if(text is not null){
            if (User.Identity.IsAuthenticated && User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            {
                allRizz.Add(_RizzCollection.FindRizzBasedOnText(text));
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
                allRizz = _RizzCollection.GetAllRizz(true);
            }
            else
            {
                allRizz = _RizzCollection.GetAllRizz(false);
            }
        }
        if(allRizz != null && allRizz.Count != 0){
            return Ok(allRizz);
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