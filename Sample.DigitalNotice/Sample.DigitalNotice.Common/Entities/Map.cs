namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class Map : Notice
{
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<MapItem> Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<TemplateItem> TemplateItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool WithNameColumn { get; set; }
}
