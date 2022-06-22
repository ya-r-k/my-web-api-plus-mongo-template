using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;

namespace Sample.DigitalNotice.Bll.Services;

/// <summary>
/// 
/// </summary>
public class DiaryService : IDiaryService
{
    private readonly IDiaryRepository diaryRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diaryRepository"></param>
    public DiaryService(IDiaryRepository diaryRepository)
    {
        this.diaryRepository = diaryRepository;
    }

    /// <inheritdoc />
    public Task<Diary> Create(DiaryRequestModel model)
    {
        return diaryRepository.Create(model);
    }

    /// <inheritdoc />
    public Task<bool> Delete(Guid diaryId)
    {
        return diaryRepository.Delete(diaryId);
    }

    /// <inheritdoc />
    public Task<IEnumerable<Diary>> GetByPage(GetByPageRequestModel model)
    {
        return diaryRepository.GetByPage(model);
    }

    /// <inheritdoc />
    public Task<Diary> GetById(Guid diaryId)
    {
        return diaryRepository.GetById(diaryId);
    }

    /// <inheritdoc />
    public Task<bool> Update(Guid diaryId, DiaryRequestModel model)
    {
        return diaryRepository.Update(diaryId, model);
    }
}
