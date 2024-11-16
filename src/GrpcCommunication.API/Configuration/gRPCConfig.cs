using GrpcCommunication.API.ProtoServices;

namespace GrpcCommunication.API.Configuration;

public static class GrpcConfig
{

    public static IServiceCollection AddGrpcConfiguration(this IServiceCollection services)
    {
        services.AddGrpc();

        return services;
    }

    public static WebApplication UseGrpcConfiguration(this WebApplication app)
    {

        app.MapGrpcService<ProductService>();

        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Servidor rodando tanto gRPC quanto APIs REST!");
        });

        return app;

    }
}



