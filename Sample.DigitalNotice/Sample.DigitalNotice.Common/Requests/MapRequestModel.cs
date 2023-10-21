using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// Represents a map request model.
/// </summary>
public class MapRequestModel
{
    /// <summary>
    /// Gets or sets the name of the map.
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the map.
    /// </summary>
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the status of the map.
    /// </summary>
    [EnumDataType(typeof(NoticeStatus))]
    public NoticeStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the type of the map.
    /// </summary>
    [EnumDataType(typeof(NoticeType))]
    public NoticeType? Type { get; set; }

    /// <summary>
    /// Gets or sets the collection of items within the map.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<MapItem> Items { get; set; }

    /// <summary>
    /// Gets or sets the collection of template items within the map.
    /// </summary>
    [MinLength(0)]
    public IEnumerable<TemplateItem> TemplateItems { get; set; }
}
