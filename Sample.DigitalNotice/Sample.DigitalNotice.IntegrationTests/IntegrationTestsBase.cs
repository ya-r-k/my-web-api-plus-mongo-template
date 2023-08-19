using Microsoft.Extensions.DependencyInjection;
using Sample.DigitalNotice.IntegrationTests.DataAccessors;

namespace Sample.DigitalNotice.IntegrationTests;

internal abstract class IntegrationTestBase : IDisposable
{
    protected readonly CustomWebApplicationFactory factory;
    protected readonly HttpClient httpClient;

    protected IntegrationTestBase()
    {
        factory = new CustomWebApplicationFactory();
        httpClient = factory.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost");

        var serviceProvider = factory.Services;

        DiaryAccessor = serviceProvider.GetRequiredService<IDiaryDataAccessor>();
    }

    protected IDiaryDataAccessor DiaryAccessor { get; set; }

    public void Dispose()
    {
        DiaryAccessor.Clear();
        httpClient?.Dispose();
        factory.Server?.Dispose();
    }
}
