using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Infrastructure;
using Sample.DigitalNotice.Common.Requests;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sample.DigitalNotice.Common.Enums;

namespace Sample.DigitalNotice.Dal.Repositories;

/// <summary>
/// 
/// </summary>
public class MapRepository : IMapRepository
{
    private const string MapsCollectionName = "Maps";

    private readonly IMongoCollection<Map> maps;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public MapRepository(IOptions<MongoDbSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);

        maps = mongoDatabase.GetCollection<Map>(MapsCollectionName);
    }

    /// <inheritdoc />
    public async Task<Map> Create(MapRequestModel model)
    {
        var map = new Map
        {
            Name = model.Name,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            Status = model.Status ?? NoticeStatus.Draft,
            Type = model.Type,
            Items = model.Items,
            TemplateItems = model.TemplateItems,
        };

        await maps.InsertOneAsync(map);

        return map;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(Guid mapId)
    {
        var filter = Builders<Map>.Filter.Eq(item => item.Id, mapId);
        var result = await maps.DeleteOneAsync(filter);

        return result is not null && result.DeletedCount > 0;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Map>> GetByPage(GetByPageRequestModel model)
    {
        var filter = Builders<Map>.Filter.Empty;

        if (model.LastViewedId != Guid.Empty)
        {
            filter = Builders<Map>.Filter.Gt(item => item.Id, model.LastViewedId);
        }

        var options = new FindOptions<Map>
        {
            Limit = model.PageSize,
            Sort = Builders<Map>.Sort.Ascending(item => item.Id),
        };

        var cursor = await maps.FindAsync(filter, options);

        return await cursor.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Map> GetById(Guid mapId)
    {
        var filter = Builders<Map>.Filter.Eq(item => item.Id, mapId);
        var cursor = await maps.FindAsync(filter);

        return await cursor.SingleOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<bool> Update(Guid mapId, MapRequestModel model)
    {
        var mapIn = await GetById(mapId);

        if (mapIn is null)
        {
            return false;
        }

        var filter = Builders<Map>.Filter.Eq(item => item.Id, mapId);

        mapIn.Name = model.Name ?? mapIn.Name;
        mapIn.Description = model.Description ?? mapIn.Description;
        mapIn.Type = model.Type ?? mapIn.Type;
        mapIn.Status = model.Status ?? mapIn.Status;
        mapIn.Items = model.Items ?? mapIn.Items;
        mapIn.TemplateItems = model.TemplateItems ?? mapIn.TemplateItems;

        var result = await maps.ReplaceOneAsync(filter, mapIn);

        return result is not null && result.MatchedCount > 0;
    }
}
