using ItequiaTalkCSOM.Interfaces;
using ItequiaTalkCSOM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItequiaTalkCSOM.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CSOMController : ControllerBase
  {


    private readonly ICSOMService _csomService;

    public CSOMController(ICSOMService csomService)
    {
      _csomService = csomService;
    }

    [HttpGet]
    [Route("ListInfo")]

    public async Task<ActionResult<ListInfo>> ListInfo([FromQuery] string siteUrl, [FromQuery] string listName)
    {
      try
      {
        ListInfo result = await _csomService.GetListInfo(siteUrl, listName);
        return Ok(result);
      }
      catch(Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpGet]
    [Route("ListItems")]

    public async Task<ActionResult<List<ItemInfo>>> ListItems([FromQuery] string siteUrl, [FromQuery] string listName)
    {
      try
      {
        List<ItemInfo> result = await _csomService.GetListItems(siteUrl, listName);
        return Ok(result);
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpGet]
    [Route("ListItemsInLargeList")]

    public async Task<ActionResult<List<ItemInfo>>> ListItemsInLargeList([FromQuery] string siteUrl, [FromQuery] string listName)
    {
      try
      {
        List<ItemInfo> result = await _csomService.GetListItemsInLargeList(siteUrl, listName);
        return Ok(result);
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpGet]
    [Route("ListItemsFiltered")]

    public async Task<ActionResult<List<ItemInfo>>> ListItemsFiltered([FromQuery] string siteUrl, [FromQuery] string listName)
    {
      try
      {
        List<ItemInfo> result = await _csomService.GetFilteredItems(siteUrl, listName);
        return Ok(result);
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpPost]
    [Route("CreateItem")]

    public async Task<ActionResult> CreateItem([FromBody] TalkListItem item)
    {
      try
      {
        await _csomService.CreateListItem(item);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpPatch]
    [Route("UpdateItem/{id}")]

    public async Task<ActionResult> UpdateItem(int id, [FromBody] TalkListItem item)
    {
      try
      {
        await _csomService.UpdateListItem(item, id);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpDelete]
    [Route("DeleteItem/{id}")]

    public async Task<ActionResult> DeleteItem(int id, [FromBody] TalkListItem item)
    {
      try
      {
        await _csomService.DeleteListItem(item, id);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpGet]
    [Route("Permissions")]

    public async Task<ActionResult<PermissionList>> Permissions([FromQuery] string siteUrl, [FromQuery] string listName, [FromQuery] int id)
    {
      try
      {
        PermissionList result = await _csomService.GetPermissions(siteUrl, listName, id);
        return Ok(result);
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpPatch]
    [Route("BreakOrInherit/{id}/{breakOrInherit}")]

    public async Task<ActionResult> BreakOrInherit(int id, bool breakOrInherit, [FromBody] TalkListItem item)
    {
      try
      {
        await _csomService.BreakInheritOrInheritAgain(id, breakOrInherit, item);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpPatch]
    [Route("GivePermission")]

    public async Task<ActionResult> GivePermission([FromBody] PermissionAndRole item)
    {
      try
      {
        await _csomService.GivePermissions(item);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }

    [HttpPatch]
    [Route("DeletePermission")]

    public async Task<ActionResult> DeletePermission([FromBody] PermissionAndRole item)
    {
      try
      {
        await _csomService.DeletePermissions(item);
        return Ok();
      }
      catch (Exception Ex)
      {
        return BadRequest("Error: " + Ex.Message);
      }
    }
  }
}
