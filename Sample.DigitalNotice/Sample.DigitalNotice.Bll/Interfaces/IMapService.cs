using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Bll.Interfaces;

/// <summary>
/// Represents a service for managing maps.
/// </summary>
public interface IMapService
{
    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <param name="model">The map request model.</param>
    /// <returns>The created map.</returns>
    Task<Map> Create(MapRequestModel model);

    /// <summary>
    /// Deletes a map by its ID.
    /// </summary>
    /// <param name="mapId">The ID of the map to delete.</param>
    /// <returns>A boolean indicating if the deletion was successful.</returns>
    Task<bool> Delete(Guid mapId);

    /// <summary>
    /// Gets a page of maps.
    /// </summary>
    /// <param name="model">The query model for pagination.</param>
    /// <returns>A collection of maps.</returns>
    Task<IEnumerable<Map>> GetByPage(GetByPageQueryModel model);

    /// <summary>
    /// Gets a map by its ID.
    /// </summary>
    /// <param name="mapId">The ID of the map to get.</param>
    /// <returns>The map.</returns>
    Task<Map> GetById(Guid mapId);

    /// <summary>
    /// Updates a map by its ID.
    /// </summary>
    /// <param name="mapId">The ID of the map to update.</param>
    /// <param name="model">The map request model.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> Update(Guid mapId, MapRequestModel model);
}
