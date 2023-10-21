using Sample.DigitalNotice.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents a note.
/// </summary>
public class Note
{
    /// <summary>
    /// Gets or sets the name of the note.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the content of the note.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the note.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last edited date of the note.
    /// </summary>
    public DateTime? EditedAt { get; set; }

    /// <summary>
    /// Gets or sets the status of the note.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoteStatus? Status { get; set; }
}
