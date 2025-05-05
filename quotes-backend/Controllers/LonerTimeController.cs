using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class LonerTimeController : ControllerBase
{
    private readonly LonerCollection _lonerCollection;
    private readonly ChartGenerator _chartGenerator;
    public LonerTimeController()
    {
        _lonerCollection = new LonerCollection();
        _chartGenerator = new ChartGenerator();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get Loner Time (Requires AUTH)", Description = "Get all Loners from the db", Tags = new[] { "LonerTime" })]
    public IActionResult getLoners()
    {
        List<Loner>? loners = _lonerCollection.GetAllLoners();
        if (loners == null)
        {
            return BadRequest();
        }

        if (loners.Count == 0)
        {
            return NoContent();
        }

        return Ok(loners);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Add Loner Time (Requires AUTH)", Description = "Post the Loner Time", Tags = new[] { "LonerTime" })]
    public IActionResult postTime([FromBody] Loner loner)
    {
        bool posted = _lonerCollection.PostTime(loner);
        if (posted)
        {
            return Ok();
        }
        return BadRequest();
    }
    [HttpGet("graph")]
    [SwaggerOperation(Summary = "Get Loner Time (Requires AUTH)", Description = "Get all Loners from the db", Tags = new[] { "LonerTime" })]
    public IActionResult graphImage()
    {
        List<Loner>? loners = _lonerCollection.GetAllLoners();
        if (loners == null)
        {
            return BadRequest();
        }
        if (loners.Count == 0)
        {
            return NoContent();
        }
        byte[] imageBytes = _chartGenerator.generateImage(loners);
        return Ok(imageBytes);
    }
}