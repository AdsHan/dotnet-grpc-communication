using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GrpcCommunication.API.gRPC.Interceptors;

public class ServerLoggingInterceptor : Interceptor
{
    private readonly ILogger<ServerLoggingInterceptor> _logger;

    public ServerLoggingInterceptor(ILogger<ServerLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Iniciando chamada gRPC: {Method}", context.Method);

        try
        {
            var response = await continuation(request, context);

            _logger.LogInformation("Chamada gRPC concluída com sucesso: {Method}", context.Method);
            return response;
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "Erro gRPC: {Method}. Status: {StatusCode}", context.Method, ex.StatusCode);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado em {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, "Erro inesperado"));
        }
    }
}