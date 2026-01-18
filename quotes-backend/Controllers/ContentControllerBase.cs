using System.Collections.Generic;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Logic;
using Microsoft.AspNetCore.Authorization;

namespace quotes_backend.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public abstract class ContentControllerBase : ControllerBase
{
    protected readonly ContentCollection Collection;
    protected readonly string ItemName;

    protected ContentControllerBase(ContentCollection collection, string itemName)
    {
        Collection = collection;
        ItemName = itemName;
    }

    [HttpGet("Random")]
    public virtual IActionResult GetRandom(bool withContext, bool asObject)
    {
        if (withContext && !Rights.hasRights(User))
        {
            return Unauthorized($"Access denied: please log in with appropriate permissions to view context for {ItemName}.");
        }

        object? item = Collection.GetRandom(withContext, asObject);

        if (item == null || item.ToString() == string.Empty)
        {
            return BadRequest($"An error occurred while fetching the {ItemName}. Please contact support.");
        }

        return Ok(item);
    }

    protected IActionResult GetAllItems(string? text, string? context, string? id)
    {
        if ((text is not null && context is not null) || (text is not null && id is not null) || (context is not null && id is not null))
        {
            return BadRequest("You can only provide one of the following params: `text`, `context`, or `id`.");
        }

        List<Quote> items = new List<Quote>();

        if (id is not null)
        {
            var found = Collection.GetById(id);
            if (found != null && found.text != null) items.Add(found);
            
            if (items.Count != 0) return Ok(items);
            return NoContent();
        }

        if (text is not null)
        {
            if (Rights.hasRights(User))
            {
                var found = Collection.FindByText(text);
                if (found != null && found.text != null) items.Add(found);
                
                if (items.Count != 0) return Ok(items);
                return NoContent();
            }
            return Unauthorized("Please Authenticate to retrieve the context");
        }

        if (context is not null)
        {
            if (Rights.hasRights(User))
            {
                var found = Collection.FindByContext(context);
                if (found != null && found.text != null) items.Add(found);

                if (items.Count != 0) return Ok(items);
                return NoContent();
            }
            return Unauthorized("Please Authenticate to retrieve the context");
        }

        items = Collection.GetAll(Rights.hasRights(User));
        
        if (items.Count != 0) return Ok(items);
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public virtual IActionResult Create([FromBody] QuoteDTOPost dto)
    {
        if (Rights.hasRights(User))
        {
            Quote? createdItem = Collection.New(dto);
            if (createdItem != null) return Ok(createdItem);
            return BadRequest();
        }
        return Forbid();
    }

    [HttpPut("{id}")]
    [Authorize]
    public virtual IActionResult Update(string id, [FromBody] Quote item)
    {
        if (Rights.hasRights(User))
        {
            bool updated = Collection.Update(id, item);
            if (updated) return Ok();
            return BadRequest();
        }
        return Forbid();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public virtual IActionResult Delete(string id)
    {
        if (Rights.hasRights(User))
        {
            bool deleted = Collection.Delete(id);
            if (deleted) return Ok();
            return BadRequest();
        }
        return Forbid();
    }
}
