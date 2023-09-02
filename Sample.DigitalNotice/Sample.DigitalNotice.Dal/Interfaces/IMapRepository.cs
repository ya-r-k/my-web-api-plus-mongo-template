using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Dal.Interfaces.Repositories;

/// <summary>
/// 
/// </summary>
public interface IMapRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Map> Create(MapRequestModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    Task<bool> Delete(Guid mapId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IEnumerable<Map>> GetByPage(GetByPageQueryModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    Task<Map> GetById(Guid mapId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Update(Guid mapId, MapRequestModel model);
}
