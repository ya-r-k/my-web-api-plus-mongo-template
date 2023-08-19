using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.DigitalNotice.IntegrationTests.DataAccessors;

namespace Sample.DigitalNotice.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionStringKey = "MongoDbSettings:ConnectionString";
    private const string DatabaseNameKey = "MongoDbSettings:DatabaseName";
    private const string ConnectionStringValue = "mongodb://localhost:27017";
    private const string DatabaseNameValue = "DigitalNoticeDbTest";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(builder =>
        {
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                [ConnectionStringKey] = ConnectionStringValue,
                [DatabaseNameKey] = DatabaseNameValue,
            });
        });

        builder.ConfigureServices(services =>
        {
            AddDataAccessors(services);
        });

        builder.UseEnvironment("Testing");
    }

    private static void AddDataAccessors(IServiceCollection services)
    {
        services.AddScoped<IDiaryDataAccessor, DiaryDataAccessor>();
    }
}
