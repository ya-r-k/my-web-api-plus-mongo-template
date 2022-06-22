using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;

namespace Sample.DigitalNotice.Bll.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDiaryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Diary> Create(DiaryRequestModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diaryId"></param>
    /// <returns></returns>
    Task<bool> Delete(Guid diaryId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Diary>> GetByPage(GetByPageRequestModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diaryId"></param>
    /// <returns></returns>
    Task<Diary> GetById(Guid diaryId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="diaryId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Update(Guid diaryId, DiaryRequestModel model);
}
