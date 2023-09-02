using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// Create diary model.
/// </summary>
public class DiaryRequestModel
{
    /// <summary>
    /// Diary name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Diary description.
    /// </summary>
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    /// <summary>
    /// Notice status.
    /// </summary>
    [EnumDataType(typeof(NoticeStatus))]
    public NoticeStatus? Status { get; set; }

    /// <summary>
    /// Notice type.
    /// </summary>
    [EnumDataType(typeof(NoticeType))]
    public NoticeType? Type { get; set; }

    /// <summary>
    /// Diary notes collection.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<Note> Notes { get; set; }
}
