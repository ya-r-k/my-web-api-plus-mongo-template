namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents a group of notes.
/// </summary>
public class NoteGroup
{
    /// <summary>
    /// Gets or sets the name of the note group.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the notes within the group.
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }
}
