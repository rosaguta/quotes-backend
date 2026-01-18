using System.Collections.Generic;
using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class QuotesController : ContentControllerBase
{
    public QuotesController() : base(new ContentCollection(Factory.DalFactory.GetQuoteDal()), "Quote")
    {
    }

    [SwaggerOperation(
        Summary = "Gets random Quote (requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("Random")]
    public override IActionResult GetRandom(bool withContext, bool asObject)
    {
        return base.GetRandom(withContext, asObject);
    }

    [SwaggerOperation(
        Summary = "Gets all quotes from database (requires AUTH)",
        Description = "When the text field is filled in, the API will try to find the requested quote. Only works with the correct rights"
    )]
    [HttpGet]
    [Route("/Quotes")]
    public IActionResult AllQuotes(string? text, string? context, string? id)
    {
        return base.GetAllItems(text, context, id);
    }

    [SwaggerOperation(
        Summary = "Adds a new Quote (requires AUTH)",
        Description = "Requires special permissions to create a new quote"
    )]
    [HttpPost(Name = "NewQuote"), Authorize]
    public override IActionResult Create([FromBody] QuoteDTOPost quote)
    {
        return base.Create(quote);
    }

    [SwaggerOperation(
        Summary = "Edits a quote (requires AUTH)",
        Description = "Requires special permissions to update"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public override IActionResult Update(string id, [FromBody] Quote quote)
    {
        return base.Update(id, quote);
    }

    [SwaggerOperation(
        Summary = "Deletes a Quote (requires AUTH)",
        Description = "Requires special permissions to delete"
    )]
    [HttpDelete]
    [Route("{id}"), Authorize]
    public override IActionResult Delete(string id)
    {
        return base.Delete(id);
    }
}