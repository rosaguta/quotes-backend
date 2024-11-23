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
    public LonerTimeController()
    {
        _lonerCollection = new LonerCollection();
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Add Loner Time", Description = "Post the Loner Time", Tags = new[] { "LonerTime" })]
    public IActionResult postTime([FromBody] Loner loner)
    {
        bool posted = _lonerCollection.PostTime(loner);
        if (posted)
        {
            return Ok();
        }
        return BadRequest();
    }
}