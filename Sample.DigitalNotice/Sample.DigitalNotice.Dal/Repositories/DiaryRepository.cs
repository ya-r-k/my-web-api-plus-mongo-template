using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Infrastructure;
using Sample.DigitalNotice.Common.Requests;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sample.DigitalNotice.Common.Enums;

namespace Sample.DigitalNotice.Dal.Repositories;

/// <summary>
/// 
/// </summary>
public class DiaryRepository : IDiaryRepository
{
    private const string DiariesCollectionName = "Diaries";

    private readonly IMongoCollection<Diary> diaries;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public DiaryRepository(IOptions<MongoDbSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);

        diaries = mongoDatabase.GetCollection<Diary>(DiariesCollectionName);
    }

    /// <inheritdoc />
    public async Task<Diary> Create(DiaryRequestModel model)
    {
        var diary = new Diary
        {
            Name = model.Name,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            Status = model.Status ?? NoticeStatus.Draft,
            Type = model.Type,
            Notes = model.Notes,
        };

        await diaries.InsertOneAsync(diary);

        return diary;
    }

    /// <inheritdoc />
    public async Task<bool> Delete(Guid diaryId)
    {
        var filter = Builders<Diary>.Filter.Eq(item => item.Id, diaryId);
        var result = await diaries.DeleteOneAsync(filter);

        return result is not null && result.DeletedCount > 0;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Diary>> GetByPage(GetByPageRequestModel model)
    {
        var filter = Builders<Diary>.Filter.Empty;

        if (model.LastViewedId != Guid.Empty)
        {
            filter = Builders<Diary>.Filter.Gt(item => item.Id, model.LastViewedId);
        }

        var options = new FindOptions<Diary>
        {
            Limit = model.PageSize,
            Sort = Builders<Diary>.Sort.Ascending(item => item.Id),
        };

        var cursor = await diaries.FindAsync(filter, options);

        return await cursor.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Diary> GetById(Guid diaryId)
    {
        var filter = Builders<Diary>.Filter.Eq(item => item.Id, diaryId);
        var cursor = await diaries.FindAsync(filter);

        return await cursor.SingleOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<bool> Update(Guid diaryId, DiaryRequestModel model)
    {
        var diaryIn = await GetById(diaryId);

        if (diaryIn is null)
        {
            return false;
        }

        var filter = Builders<Diary>.Filter.Eq(item => item.Id, diaryId);

        diaryIn.Name = model.Name ?? diaryIn.Name;
        diaryIn.Description = model.Description ?? diaryIn.Description;
        diaryIn.Type = model.Type ?? diaryIn.Type;
        diaryIn.Status = model.Status ?? diaryIn.Status;
        diaryIn.Notes = model.Notes ?? diaryIn.Notes;

        var result = await diaries.ReplaceOneAsync(filter, diaryIn);

        return result is not null && result.MatchedCount > 0;
    }
}
