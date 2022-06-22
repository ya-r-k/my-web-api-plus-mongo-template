namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class MapItemNotes
{
    /// <summary>
    /// 
    /// </summary>
    public uint TemplateItemNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<NoteGroup> NoteGroups { get; set; }
}
