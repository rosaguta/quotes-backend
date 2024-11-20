using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]
public class LonerTimeController : ControllerBase
{
    private readonly LonerCollection _lonerCollection;
    public LonerTimeController()
    {
        _lonerCollection = new LonerCollection();
    }
    
    [HttpPost]
    [Authorize]
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