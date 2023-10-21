namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents an item in a map.
/// </summary>
public class MapItem
{
    /// <summary>
    /// Gets or sets the name of the map item.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the notes associated with the map item.
    /// </summary>
    public IEnumerable<MapItemNotes> Notes { get; set; }
}
