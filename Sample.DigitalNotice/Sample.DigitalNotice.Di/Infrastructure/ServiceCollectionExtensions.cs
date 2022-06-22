using Sample.DigitalNotice.Bll.Interfaces;
using Sample.DigitalNotice.Bll.Services;
using Sample.DigitalNotice.Dal.Interfaces.Repositories;
using Sample.DigitalNotice.Dal.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.DigitalNotice.Di.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDiaryRepository, DiaryRepository>();
        services.AddTransient<IMapRepository, MapRepository>();

        services.AddTransient<IDiaryService, DiaryService>();
        services.AddTransient<IMapService, MapService>();

        return services;
    }
}
