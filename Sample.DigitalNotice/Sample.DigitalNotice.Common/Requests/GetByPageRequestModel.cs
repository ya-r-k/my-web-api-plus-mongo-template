using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// 
/// </summary>
public class GetByPageRequestModel
{
    /// <summary>
    /// 
    /// </summary>
    public Guid LastViewedId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Range(1, byte.MaxValue)]
    public byte PageSize { get; set; }
}
