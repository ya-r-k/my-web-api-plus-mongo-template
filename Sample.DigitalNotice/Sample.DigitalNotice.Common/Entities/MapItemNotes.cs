namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents notes associated with a map item.
/// </summary>
public class MapItemNotes
{
    /// <summary>
    /// Gets or sets the template item number.
    /// </summary>
    public uint TemplateItemNumber { get; set; }

    /// <summary>
    /// Gets or sets the individual notes.
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }

    /// <summary>
    /// Gets or sets the note groups.
    /// </summary>
    public IEnumerable<NoteGroup> NoteGroups { get; set; }
}
