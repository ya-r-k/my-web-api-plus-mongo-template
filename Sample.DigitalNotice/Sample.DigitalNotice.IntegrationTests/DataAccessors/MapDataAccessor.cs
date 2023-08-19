using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Infrastructure;

namespace Sample.DigitalNotice.IntegrationTests.DataAccessors;

/// <summary>
/// Provides access to the maps collection in the MongoDB database.
/// </summary>
internal class MapDataAccessor : IMapDataAccessor
{
    private readonly IMongoCollection<Map> collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapDataAccessor"/> class.
    /// </summary>
    /// <param name="mongoDbSettings">The MongoDB settings to use for the data accessor.</param>
    public MapDataAccessor(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

        collection = database.GetCollection<Map>("Maps");
    }

    /// <inheritdoc />
    public IEnumerable<Map> GetAll()
    {
        return collection.Find(_ => true).ToList();
    }

    /// <inheritdoc />
    public Map GetById(Guid mapId)
    {
        var filter = Builders<Map>.Filter.Eq(item => item.Id, mapId);

        return collection.Find(filter).SingleOrDefault();
    }

    /// <inheritdoc />
    public IMapDataAccessor Push(params Map[] maps)
    {
        collection.InsertMany(maps);

        return this;
    }

    /// <inheritdoc />
    public void Clear()
    {
        collection.DeleteMany(_ => true);
    }
}
