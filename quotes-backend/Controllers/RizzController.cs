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
}