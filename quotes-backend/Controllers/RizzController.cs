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
        Summary = "Gets random Rizz (if there are any)"
    )]
    [HttpGet]
    [Route("/Rizzes/Random")]
    public IActionResult GetRandom()
    {
        string? rizz = _RizzCollection.GetRandomRizz();
        if (rizz is not null)
        {
            return Ok(rizz);
        }
        return NoContent();
    }
    [SwaggerOperation(
        Summary = "Gets all Rizz"
    )]
    [HttpGet]
    [Route("/Rizzes")]
    public IActionResult GetAllRizz()
    {
        List<Quote>? rizzes = _RizzCollection.GetAllRizz();
        if (rizzes[0] is not null)
        {
            return Ok(rizzes);
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