using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// Map request model.
/// </summary>
public class MapRequestModel
{
    /// <summary>
    /// Map name.
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Map description.
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
    /// Map items.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<MapItem> Items { get; set; }

    /// <summary>
    /// Map template items.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<TemplateItem> TemplateItems { get; set; }
}
