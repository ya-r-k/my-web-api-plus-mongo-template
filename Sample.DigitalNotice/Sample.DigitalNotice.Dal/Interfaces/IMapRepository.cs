using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Dal.Interfaces.Repositories;

/// <summary>
/// Repository class for managing maps.
/// </summary>
public interface IMapRepository
{
    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <param name="model">The map request model.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created map.</returns>
    Task<Map> Create(MapRequestModel model);

    /// <summary>
    /// Deletes a map with the specified ID.
    /// </summary>
    /// <param name="mapId">The ID of the map to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the map was successfully deleted.</returns>
    Task<bool> Delete(Guid mapId);

    /// <summary>
    /// Retrieves a collection of maps based on the specified query model.
    /// </summary>
    /// <param name="model">The query model specifying the page information.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the collection of maps.</returns>
    Task<IEnumerable<Map>> GetByPage(GetByPageQueryModel model);

    /// <summary>
    /// Retrieves a map with the specified ID.
    /// </summary>
    /// <param name="mapId">The ID of the map to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved map.</returns>
    Task<Map> GetById(Guid mapId);

    /// <summary>
    /// Updates a map with the specified ID using the provided model.
    /// </summary>
    /// <param name="mapId">The ID of the map to update.</param>
    /// <param name="model">The map request model.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the map was successfully updated.</returns>
    Task<bool> Update(Guid mapId, MapRequestModel model);
}
