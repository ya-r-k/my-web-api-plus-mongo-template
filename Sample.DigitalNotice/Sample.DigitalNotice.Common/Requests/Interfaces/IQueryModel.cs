namespace Sample.DigitalNotice.Common.Requests.Interfaces;

/// <summary>
/// Represents an interface for query models that can be converted to a URL query string.
/// </summary>
public interface IQueryModel
{
    /// <summary>
    /// Converts the query model to a URL query string.
    /// </summary>
    /// <returns>The query model as a URL query string.</returns>
    string ToQueryString();
}
