using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// Represents a diary model.
/// </summary>
public class DiaryRequestModel
{
    /// <summary>
    /// Gets or sets the name of the diary.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the diary.
    /// </summary>
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the status of the diary.
    /// </summary>
    [EnumDataType(typeof(NoticeStatus))]
    public NoticeStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the type of the diary.
    /// </summary>
    [EnumDataType(typeof(NoticeType))]
    public NoticeType? Type { get; set; }

    /// <summary>
    /// Gets or sets the collection of notes within the diary.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<Note> Notes { get; set; }
}
