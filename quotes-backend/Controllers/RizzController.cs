using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace quotes_backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RizzController : ContentControllerBase
{
    public RizzController() : base(new ContentCollection(Factory.DalFactory.GetRizzDal()), "Rizz")
    {
    }

    [SwaggerOperation(
        Summary = "Gets random Rizz (if there are any) (Requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("/Rizzes/Random")]
    public override IActionResult GetRandom(bool withContext, bool asObject)
    {
        return base.GetRandom(withContext, asObject);
    }

    [SwaggerOperation(
        Summary = "Gets all Rizz (Requires AUTH)",
        Description = "Requires special permissions to retrieve the context"
    )]
    [HttpGet]
    [Route("/Rizzes")]
    public IActionResult GetAllRizz(string? text, string? context, string? id)
    {
        return base.GetAllItems(text, context, id);
    }

    [SwaggerOperation(
        Summary = "Adds a new Rizz (Requires AUTH)",
        Description = "Requires special permissions to create some new rizz"
    )]
    [HttpPost]
    [Route("/Rizzes"), Authorize]
    public override IActionResult Create([FromBody] QuoteDTOPost rizzpost)
    {
        return base.Create(rizzpost);
    }

    [SwaggerOperation(
        Summary = "Edits a Rizz (Requires AUTH)",
        Description = "Requires AUTH"
    )]
    [HttpPut]
    [Route("/Rizzes/{id}"), Authorize]
    public override IActionResult Update(string id, [FromBody] Quote rizz)
    {
        return base.Update(id, rizz);
    }

    [HttpDelete]
    [Route("/Rizzes/{id}")]
    [SwaggerOperation(Summary = "Deletes a single rizz Requires AUTH",
        Description = "Requires special permissions to delete some rizz")]
    public override IActionResult Delete(string id)
    {
        return base.Delete(id);
    }
}