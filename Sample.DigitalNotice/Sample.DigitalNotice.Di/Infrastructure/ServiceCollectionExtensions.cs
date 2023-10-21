using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Bll.Services;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;
using Sample.DigitalNotice.Dal.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.DigitalNotice.Di.Infrastructure;

/// <summary>
/// Provides extension methods for configuring services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDiaryRepository, DiaryRepository>();
        services.AddTransient<IMapRepository, MapRepository>();

        services.AddTransient<IDiaryService, DiaryService>();
        services.AddTransient<IMapService, MapService>();

        return services;
    }
}
