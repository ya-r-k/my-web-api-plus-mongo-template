using Sample.DigitalNotice.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// Represents a notice.
/// </summary>
public class Notice
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [BsonId(IdGenerator = typeof(AscendingGuidGenerator))]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the date of creation.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoticeStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoticeType? Type { get; set; }
}
