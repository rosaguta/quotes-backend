using System.Collections.Generic;
using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class InsultsController : ContentControllerBase
{
    public InsultsController() : base(new ContentCollection(Factory.DalFactory.GetInsultDal()), "Insult")
    {
    }

    [SwaggerOperation(
        Summary = "Gets random Insult (requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("Random")]
    public override IActionResult GetRandom(bool withContext, bool asObject)
    {
        return base.GetRandom(withContext, asObject);
    }

    [SwaggerOperation(
        Summary = "Gets all insults from database (requires AUTH)",
        Description = "Requires special permissions to retrieve the `context` OR `context`"
    )]
    [HttpGet]
    [Route("/Insults")]
    public IActionResult AllInsults(string? text, string? context, string? id)
    {
        return base.GetAllItems(text, context, id);
    }

    [SwaggerOperation(
        Summary = "Adds a new Insult (requires AUTH)"
    )]
    [HttpPost(Name = "NewInsult"), Authorize]
    public override IActionResult Create([FromBody] QuoteDTOPost insult)
    {
        return base.Create(insult);
    }

    [SwaggerOperation(
        Summary = "Edits an Insult (requires AUTH)"
    )]
    [HttpPut]
    [Route("{id}"), Authorize]
    public override IActionResult Update(string id, [FromBody] Quote quote)
    {
        return base.Update(id, quote);
    }

    [SwaggerOperation(
        Summary = "Deletes an Insult (requires AUTH)"
    )]
    [HttpDelete]
    [Route("{id}"), Authorize]
    public override IActionResult Delete(string id)
    {
        return base.Delete(id);
    }
}