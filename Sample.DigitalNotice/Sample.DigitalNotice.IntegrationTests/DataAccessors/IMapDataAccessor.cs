using Sample.DigitalNotice.Common.Entities;

namespace Sample.DigitalNotice.IntegrationTests.DataAccessors;

/// <summary>
/// Provides access to the maps collection in the database.
/// </summary>
internal interface IMapDataAccessor
{
    /// <summary>
    /// Gets all maps from the database.
    /// </summary>
    /// <returns>An enumerable collection of all maps in the database.</returns>
    IEnumerable<Map> GetAll();

    /// <summary>
    /// Gets a map with the specified id from the database.
    /// </summary>
    /// <param name="mapId">The id of the map to get.</param>
    /// <returns>A map object with the specified id, or null if no such map exists.</returns>
    Map GetById(Guid mapId);

    /// <summary>
    /// Creates new maps in the database.
    /// </summary>
    /// <param name="maps">The maps to create.</param>
    /// <returns>The instance of the IMapDataAccessor.</returns>
    IMapDataAccessor Push(params Map[] maps);

    /// <summary>
    /// Deletes all maps from the database.
    /// </summary>
    void Clear();
}
