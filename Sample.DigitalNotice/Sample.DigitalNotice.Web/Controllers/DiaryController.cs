using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Sample.DigitalNotice.Web.Controllers;

/// <summary>
/// Provides diaries endpoints.
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
    /// <param name="diaryService"></param>
    public DiaryController(IDiaryService diaryService)
    {
        this.diaryService = diaryService;
    }

    /// <summary>
    /// Creates a Diary.
    /// </summary>
    /// <param name="request">Create diary model.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /diaries
    ///     {
    ///        "name": "New diary",
    ///        "description": "Description about new diary"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created diary</response>
    /// <response code="400">If the request is not valid</response>
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
    /// Returns collection of diaries.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /diaries
    ///
    /// </remarks>
    /// <response code="200">Returns collection of diaries</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Diary>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByPage([FromQuery] GetByPageQueryModel model)
    {
        return Ok(await diaryService.GetByPage(model));
    }

    /// <summary>
    /// Returns diary by id.
    /// </summary>
    /// <param name="diaryId">Diary id.</param>
    /// <response code="200">Returns collection of diaries.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If diary not found.</response>
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
    /// Updates diary.
    /// </summary>
    /// <param name="diaryId"></param>
    /// <param name="request">Update diary request</param>
    /// <response code="204">If diary updated successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If diary not found.</response>
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
    /// Deletes diary.
    /// </summary>
    /// <param name="diaryId"></param>
    /// <response code="204">If diary deleted successfully.</response>
    /// <response code="400">If the request is not valid.</response>
    /// <response code="404">If diary not found.</response>
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
