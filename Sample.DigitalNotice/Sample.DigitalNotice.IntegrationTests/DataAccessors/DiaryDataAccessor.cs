using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Infrastructure;

namespace Sample.DigitalNotice.IntegrationTests.DataAccessors;

/// <summary>
/// Provides access to the diaries collection in the MongoDB database.
/// </summary>
internal class DiaryDataAccessor : IDiaryDataAccessor
{
    private readonly IMongoCollection<Diary> collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiaryDataAccessor"/> class.
    /// </summary>
    /// <param name="mongoDbSettings">The MongoDB settings to use for the data accessor.</param>
    public DiaryDataAccessor(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

        collection = database.GetCollection<Diary>("Diaries");
    }

    /// <inheritdoc />
    public IEnumerable<Diary> GetAll()
    {
        return collection.Find(_ => true).ToList();
    }

    /// <inheritdoc />
    public Diary GetById(Guid diaryId)
    {
        var filter = Builders<Diary>.Filter.Eq(item => item.Id, diaryId);

        return collection.Find(filter).SingleOrDefault();
    }

    /// <inheritdoc />
    public IDiaryDataAccessor Push(params Diary[] diaries)
    {
        collection.InsertMany(diaries);

        return this;
    }

    /// <inheritdoc />
    public void Clear()
    {
        collection.DeleteMany(_ => true);
    }
}
