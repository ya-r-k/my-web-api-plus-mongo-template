using Sample.DigitalNotice.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class Note
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EditedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoteStatus? Status { get; set; }
}
