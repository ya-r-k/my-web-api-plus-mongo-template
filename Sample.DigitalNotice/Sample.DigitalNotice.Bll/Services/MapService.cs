using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;

namespace Sample.DigitalNotice.Bll.Services;

/// <summary>
/// 
/// </summary>
public class MapService : IMapService
{
    private readonly IMapRepository mapRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapRepository"></param>
    public MapService(IMapRepository mapRepository)
    {
        this.mapRepository = mapRepository;
    }

    /// <inheritdoc />
    public Task<Map> Create(MapRequestModel model)
    {
        return mapRepository.Create(model);
    }

    /// <inheritdoc />
    public Task<bool> Delete(Guid mapId)
    {
        return mapRepository.Delete(mapId);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Map>> GetByPage(GetByPageRequestModel model)
    {
        return mapRepository.GetByPage(model);
    }

    /// <inheritdoc />
    public Task<Map> GetById(Guid mapId)
    {
        return mapRepository.GetById(mapId);
    }

    /// <inheritdoc />
    public Task<bool> Update(Guid mapId, MapRequestModel model)
    {
        return mapRepository.Update(mapId, model);
    }
}
