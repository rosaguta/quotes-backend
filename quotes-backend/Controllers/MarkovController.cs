using Logic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]

public class MarkovController : ControllerBase
{
    private MarkovCollection _markovCollection;

    public MarkovController()
    {
        _markovCollection = new MarkovCollection();
    }
    [HttpGet]
    [SwaggerOperation(
        Summary = "This gets the Markov generated messages with its metadata (Requires AUTH)"
    )]
    public IActionResult GetDiscordMessages()
    {
        var data = _markovCollection.GetAllMarkovData();
        if (data.Count == 0)
        {
            return NoContent();
        }
        return Ok(data);
    }
    [HttpGet("{DiscordMessageId}")]
    [SwaggerOperation(
        Summary = "This gets the Markov generated message with its metadata based on the discord message ID (Requires AUTH)"
    )]
    public IActionResult GetDiscordMessage(string discordMessageId)
    {
        var data = _markovCollection.GetMarkovData(discordMessageId);
        return Ok(data);
    }
    [HttpPost]
    [SwaggerOperation(
        Summary = "This saves a Markov generated messages with its metadata (Requires AUTH)"
    )]
    public IActionResult PostMarkovMessage([FromBody] MarkovData markovData)
    {
        bool saved = _markovCollection.SaveMarkovData(markovData);
        if(!saved)
            return BadRequest();
        return Ok();
    }
    
}