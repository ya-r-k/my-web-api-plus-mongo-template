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
    /// Initializes a new instance of the MapController class.
    /// </summary>
    /// <param name="mapService">The map service.</param>
    public MapController(IMapService mapService)
    {
        this.mapService = mapService;
    }

    /// <summary>
    /// Creates a map.
    /// </summary>
    /// <param name="request">The create map model.</param>
    /// <remarks>
    /// This endpoint creates a new map based on the provided request model.
    /// The request should include the name and description of the map.
    /// 
    /// Sample request:
    /// 
    /// POST /maps
    /// {
    ///    "name": "New map",
    ///    "description": "Description about new map"
    /// }
    /// 
    /// </remarks>
    /// <response code="201">Returns the newly created map.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="500">If an internal server error occurs.</response>
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
    /// Returns a collection of maps.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves a collection of maps.
    /// 
    /// Sample request:
    /// 
    /// GET /maps
    /// 
    /// </remarks>
    /// <response code="200">Returns a collection of maps.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Map>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByPage([FromQuery] GetByPageQueryModel model)
    {
        return Ok(await mapService.GetByPage(model));
    }

    /// <summary>
    /// Returns a map by ID.
    /// </summary>
    /// <param name="mapId">The ID of the map.</param>
    /// <remarks>
    /// This endpoint retrieves a map based on the provided ID.
    /// 
    /// Sample request:
    /// 
    /// GET /maps/{mapId}
    /// 
    /// </remarks>
    /// <response code="200">Returns the map with the specified ID.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the map is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
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
    /// Updates a map.
    /// </summary>
    /// <param name="mapId">The ID of the map.</param>
    /// <param name="request">The update map request.</param>
    /// <remarks>
    /// This endpoint updates a map based on the provided ID and request.
    /// 
    /// Sample request:
    /// 
    /// PUT /maps/{mapId}
    /// {
    ///    "name": "Updated map",
    ///    "description": "Updated description of the map"
    /// }
    /// 
    /// </remarks>
    /// <response code="204">If the map is updated successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the map is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
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
    /// Deletes a map.
    /// </summary>
    /// <param name="mapId">The ID of the map to delete.</param>
    /// <remarks>
    /// This endpoint deletes a map based on the provided ID.
    /// 
    /// Sample request:
    /// 
    /// DELETE /maps/{mapId}
    /// 
    /// </remarks>
    /// <response code="204">If the map is deleted successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the map is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
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
