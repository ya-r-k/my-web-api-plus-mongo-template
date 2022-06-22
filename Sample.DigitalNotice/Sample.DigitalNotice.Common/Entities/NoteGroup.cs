namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class NoteGroup
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }
}
