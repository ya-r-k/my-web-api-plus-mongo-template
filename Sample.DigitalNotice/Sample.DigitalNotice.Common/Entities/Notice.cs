using Sample.DigitalNotice.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sample.DigitalNotice.Common.Entities;

/// <summary>
/// 
/// </summary>
public class Notice
{
    /// <summary>
    /// 
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoticeStatus Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public NoticeType? Type { get; set; }
}
