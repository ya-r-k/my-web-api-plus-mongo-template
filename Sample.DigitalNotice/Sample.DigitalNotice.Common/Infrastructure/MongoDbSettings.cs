namespace Sample.DigitalNotice.Common.Infrastructure;

/// <summary>
/// Represents MongoDb settings.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string DatabaseName { get; set; }
}
