using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
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

    [HttpPost]
    [Route("/Rizzes")]
    public IActionResult NewRizz([FromBody] QuoteDTOPost rizzpost)
    {
        bool created= _RizzCollection.NewRizz(rizzpost);
        if (created)
        {
            return Ok(created);
        }

        return BadRequest(created);
    }

    [HttpPut]
    [Route("/Rizzes/{id}")]
    public IActionResult UpdateRizz(string id, [FromBody] Quote rizz)
    {
        bool updated = _RizzCollection.UpdateRizz(id, rizz);
        if (updated)
        {
            return Ok(updated);
            
        }
        return BadRequest(updated);
    }

}