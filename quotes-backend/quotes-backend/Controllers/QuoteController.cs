using Microsoft.AspNetCore.Mvc;
using Logic;
namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class QuoteController : ControllerBase
{
    private QuoteCollection _quoteCollection;
    public QuoteController()
    {
        _quoteCollection = new QuoteCollection();
    }
    
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
        _quoteCollection = new QuoteCollection();
        
        bool created = _quoteCollection.NewQuote(quote);
        return created;
    }
}