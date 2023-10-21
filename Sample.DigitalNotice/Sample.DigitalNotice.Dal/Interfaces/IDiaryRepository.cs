using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Dal.Interfaces.Repositories;

/// <summary>
/// Repository class for managing diaries.
/// </summary>
public interface IDiaryRepository
{
    /// <summary>
    /// Creates a new diary.
    /// </summary>
    /// <param name="model">The diary request model.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created diary.</returns>
    Task<Diary> Create(DiaryRequestModel model);

    /// <summary>
    /// Deletes a diary with the specified ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the diary was successfully deleted.</returns>
    Task<bool> Delete(Guid diaryId);

    /// <summary>
    /// Retrieves a collection of diaries based on the specified query model.
    /// </summary>
    /// <param name="model">The query model specifying the page information.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the collection of diaries.</returns>
    Task<IEnumerable<Diary>> GetByPage(GetByPageQueryModel model);

    /// <summary>
    /// Retrieves a diary with the specified ID.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved diary.</returns>
    Task<Diary> GetById(Guid diaryId);

    /// <summary>
    /// Updates a diary with the specified ID using the provided model.
    /// </summary>
    /// <param name="diaryId">The ID of the diary to update.</param>
    /// <param name="model">The diary request model.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the diary was successfully updated.</returns>
    Task<bool> Update(Guid diaryId, DiaryRequestModel model);
}
