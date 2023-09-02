using Microsoft.AspNetCore.Http;
using Sample.DigitalNotice.Common.Requests.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sample.DigitalNotice.Common.Requests;

/// <summary>
/// Represents a request information for endpoints with paging.
/// </summary>
public class GetByPageQueryModel : IQueryModel
{
    /// <summary>
    /// Gets or sets the last viewed id.
    /// </summary>
    public Guid LastViewedId { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    [Required]
    [Range(1, byte.MaxValue)]
    public byte PageSize { get; set; }

    /// <inheritdoc />
    public string ToQueryString()
    {
        var queryString = new QueryString();

        // Check if the LastViewedId is not empty and add it to the query string
        if (LastViewedId != Guid.Empty)
        {
            queryString = queryString.Add(nameof(LastViewedId), LastViewedId.ToString());
        }

        // Add the PageSize to the query string
        queryString = queryString.Add(nameof(PageSize), PageSize.ToString());

        return queryString.Value;
    }
}
