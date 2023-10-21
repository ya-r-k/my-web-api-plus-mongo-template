using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Sample.DigitalNotice.Web.Controllers;

/// <summary>
/// Represents API endpoints for managing diaries.
/// </summary>
[ApiController]
[Route("api/diaries")]
[Produces("application/json")]
public class DiaryController : ControllerBase
{
    private readonly IDiaryService diaryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiaryController"/> class.
    /// </summary>
    /// <param name="diaryService">The diary service.</param>
    public DiaryController(IDiaryService diaryService)
    {
        this.diaryService = diaryService;
    }

    /// <summary>
    /// Creates a new diary with the provided details.
    /// </summary>
    /// <param name="request">The model containing the details of the diary to be created.</param>
    /// <remarks>
    /// This endpoint handles the logic to create a new diary based on the provided request model.
    /// The request should include the name and description of the diary.
    ///
    /// Sample request:
    ///
    /// POST /diaries
    /// {
    ///    "name": "New diary",
    ///    "description": "Description about new diary"
    /// }
    /// </remarks>
    /// <response code="201">Returns the newly created diary.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(DiaryRequestModel request)
    {
        var diary = await diaryService.Create(request);

        if (diary.Id == Guid.Empty)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { diaryId = diary.Id }, diary);
    }

    /// <summary>
    /// Returns a collection of diaries.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves a collection of diaries.
    /// 
    /// Sample request:
    /// 
    /// GET /diaries
    /// 
    /// </remarks>
    /// <response code="200">Returns a collection of diaries.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Diary>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByPage([FromQuery] GetByPageQueryModel model)
    {
        return Ok(await diaryService.GetByPage(model));
    }

    /// <summary>
    /// Returns a diary by ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary.</param>
    /// <remarks>
    /// This endpoint retrieves a diary based on the provided ID.
    /// 
    /// Sample request:
    /// 
    /// GET /diaries/{diaryId}
    /// 
    /// </remarks>
    /// <response code="200">Returns the diary with the specified ID.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the diary is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("{diaryId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Diary))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid diaryId)
    {
        if (diaryId == Guid.Empty)
        {
            return BadRequest();
        }

        var diary = await diaryService.GetById(diaryId);

        if (diary is null)
        {
            return NotFound();
        }

        return Ok(diary);
    }

    /// <summary>
    /// Updates a diary.
    /// </summary>
    /// <param name="diaryId">The ID of the diary.</param>
    /// <param name="request">The update diary request.</param>
    /// <remarks>
    /// This endpoint updates a diary based on the provided ID and request.
    /// 
    /// Sample request:
    /// 
    /// PUT /diaries/{diaryId}
    /// {
    ///    "name": "Updated diary",
    ///    "description": "Updated description of the diary"
    /// }
    /// 
    /// </remarks>
    /// <response code="204">If the diary is updated successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the diary is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("{diaryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid diaryId, DiaryRequestModel request)
    {
        if (diaryId == Guid.Empty)
        {
            return BadRequest();
        }

        if (!await diaryService.Update(diaryId, request))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a diary.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to delete.</param>
    /// <remarks>
    /// This endpoint deletes a diary based on the provided ID.
    /// 
    /// Sample request:
    /// 
    /// DELETE /diaries/{diaryId}
    /// 
    /// </remarks>
    /// <response code="204">If the diary is deleted successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If the diary is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpDelete("{diaryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid diaryId)
    {
        if (diaryId == Guid.Empty)
        {
            return BadRequest();
        }

        if (!await diaryService.Delete(diaryId))
        {
            return NotFound();
        }

        return NoContent();
    }
}
