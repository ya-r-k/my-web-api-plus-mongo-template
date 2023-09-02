using Sample.DigitalNotice.Common.Entities;

namespace Sample.DigitalNotice.IntegrationTests.DataAccessors;

/// <summary>
/// Provides access to the diaries collection in the database.
/// </summary>
internal interface IDiaryDataAccessor
{
    /// <summary>
    /// Gets all diaries from the database.
    /// </summary>
    /// <returns>An enumerable collection of all diaries in the database.</returns>
    IEnumerable<Diary> GetAll();

    /// <summary>
    /// Gets a diary with the specified id from the database.
    /// </summary>
    /// <param name="diaryId">The id of the diary to get.</param>
    /// <returns>A diary object with the specified id, or null if no such diary exists.</returns>
    Diary GetById(Guid diaryId);

    /// <summary>
    /// Creates new diaries in the database.
    /// </summary>
    /// <param name="diaries">The diaries to create.</param>
    /// <returns>The instance of the IdiaryDataAccessor.</returns>
    IDiaryDataAccessor Push(params Diary[] diaries);

    /// <summary>
    /// Deletes all diaries from the database.
    /// </summary>
    void Clear();
}
