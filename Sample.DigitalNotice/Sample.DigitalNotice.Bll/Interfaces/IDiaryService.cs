using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Bll.Interfaces;

/// <summary>
/// Represents a service for managing diaries.
/// </summary>
public interface IDiaryService
{
    /// <summary>
    /// Creates a new diary.
    /// </summary>
    /// <param name="model">The diary request model.</param>
    /// <returns>The created diary.</returns>
    Task<Diary> Create(DiaryRequestModel model);

    /// <summary>
    /// Deletes a diary by its ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to delete.</param>
    /// <returns>A boolean indicating if the deletion was successful.</returns>
    Task<bool> Delete(Guid diaryId);

    /// <summary>
    /// Gets a page of diaries.
    /// </summary>
    /// <param name="model">The query model for pagination.</param>
    /// <returns>A collection of diaries.</returns>
    Task<IEnumerable<Diary>> GetByPage(GetByPageQueryModel model);

    /// <summary>
    /// Gets a diary by its ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to get.</param>
    /// <returns>The diary.</returns>
    Task<Diary> GetById(Guid diaryId);

    /// <summary>
    /// Updates a diary by its ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to update.</param>
    /// <param name="model">The diary request model.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> Update(Guid diaryId, DiaryRequestModel model);
}
