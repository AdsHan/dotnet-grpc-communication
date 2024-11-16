using GrpcCommunication.API.Application.Services;
using GrpcCommunication.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GrpcCommunication.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase("CatalogDB"));

        services.AddTransient<ProductPopulateService>();

        return services;
    }
}
