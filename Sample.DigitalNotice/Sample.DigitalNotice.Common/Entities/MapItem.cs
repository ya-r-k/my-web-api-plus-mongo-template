namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class MapItem
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<MapItemNotes> Notes { get; set; }
}
