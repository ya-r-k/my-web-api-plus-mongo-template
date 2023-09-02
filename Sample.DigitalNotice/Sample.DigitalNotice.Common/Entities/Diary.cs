namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents a diary.
/// </summary>
public class Diary : Notice
{
    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public IEnumerable<Note> Notes { get; set; }
}
