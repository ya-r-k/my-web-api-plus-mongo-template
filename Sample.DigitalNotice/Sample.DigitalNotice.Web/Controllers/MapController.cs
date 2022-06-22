using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Sample.DigitalNotice.Web.Controllers;

/// <summary>
/// Provides maps endpoints.
/// </summary>
[ApiController]
[Route("api/maps")]
[Produces("application/json")]
public class MapController : ControllerBase
{
    private readonly IMapService mapService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapService"></param>
    public MapController(IMapService mapService)
    {
        this.mapService = mapService;
    }

    /// <summary>
    /// Creates a Map.
    /// </summary>
    /// <param name="request">Create map model.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /maps
    ///     {
    ///        "name": "New map",
    ///        "description": "Description about new map"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created map</response>
    /// <response code="400">If the request is not valid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(MapRequestModel request)
    {
        var map = await mapService.Create(request);

        if (map.Id == Guid.Empty)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { mapId = map.Id }, map);
    }

    /// <summary>
    /// Returns collection of maps.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /maps
    ///
    /// </remarks>
    /// <response code="200">Returns collection of maps</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Map>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByPage(GetByPageRequestModel model)
    {
        return Ok(await mapService.GetByPage(model));
    }

    /// <summary>
    /// Returns map by id.
    /// </summary>
    /// <param name="mapId">Map id.</param>
    /// <response code="200">Returns collection of map.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If map not found.</response>
    [HttpGet("{mapId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Map))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid mapId)
    {
        if (mapId == Guid.Empty)
        {
            return BadRequest();
        }

        var map = await mapService.GetById(mapId);

        if (map is null)
        {
            return NotFound();
        }

        return Ok(map);
    }

    /// <summary>
    /// Updates map.
    /// </summary>
    /// <param name="mapId"></param>
    /// <param name="request">Update map request</param>
    /// <response code="204">If map updated successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If map not found.</response>
    [HttpPut("{mapId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid mapId, MapRequestModel request)
    {
        if (mapId == Guid.Empty)
        {
            return BadRequest();
        }

        if (!await mapService.Update(mapId, request))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes map.
    /// </summary>
    /// <param name="mapId"></param>
    /// <response code="204">If map deleted successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If map not found.</response>
    [HttpDelete("{mapId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid mapId)
    {
        if (mapId == Guid.Empty)
        {
            return BadRequest();
        }

        if (!await mapService.Delete(mapId))
        {
            return NotFound();
        }

        return NoContent();
    }
}
