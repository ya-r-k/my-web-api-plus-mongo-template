namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents a map derived from a notice.
/// </summary>
public class Map : Notice
{
    /// <summary>
    /// Gets or sets the items associated with the map.
    /// </summary>
    public IEnumerable<MapItem> Items { get; set; }

    /// <summary>
    /// Gets or sets the template items associated with the map.
    /// </summary>
    public IEnumerable<TemplateItem> TemplateItems { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the map has a name column.
    /// </summary>
    public bool WithNameColumn { get; set; }
}
