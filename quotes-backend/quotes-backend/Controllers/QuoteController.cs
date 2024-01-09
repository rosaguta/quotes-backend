using Microsoft.AspNetCore.Mvc;
using Logic;
namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class QuoteController : ControllerBase
{
    [HttpGet(Name = "GetRandomQuote")]
    public IActionResult Get()
    {
        string? quote = new QuoteCollection().GetRandomQuote();
        if (quote is null)
        {
            return BadRequest("Something went wrong, blame Rose for this issue :3");
        }
        return Ok(quote);
    }
    [HttpPost(Name = "NewQuote")]
    public bool NewQuote([FromBody] Quote quote)
    {
        QuoteCollection quoteCollection = new QuoteCollection();
        
        bool created = quoteCollection.NewQuote(quote);
        return created;
    }
}