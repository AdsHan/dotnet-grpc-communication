using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcCommunication.Grpc;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Aguardando 10 segundos...");
        await Task.Delay(10000);

        var client = ChannelConfig();

        var product = await GetByIdAsync(client, 1);
        Console.WriteLine($"Produto encontrado: {product.Title}, Preço: {product.Price}");

        var products = await GetAllAsync(client);
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Title}, Preço: {p.Price}");
        }

        Console.ReadLine();
    }

    static Product.ProductClient ChannelConfig()
    {
        var retryPolicy = new RetryPolicy
        {
            MaxAttempts = 5, // Número máximo de tentativas
            InitialBackoff = TimeSpan.FromSeconds(1), // Tempo inicial de espera
            MaxBackoff = TimeSpan.FromSeconds(5), // Tempo máximo de espera
            BackoffMultiplier = 2, // Multiplicador exponencial
            RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.DeadlineExceeded }
        };

        var methodConfig = new MethodConfig
        {
            Names = { MethodName.Default }, // Aplica a todos os métodos
            RetryPolicy = retryPolicy
        };

        var channelOptions = new GrpcChannelOptions
        {
            ServiceConfig = new ServiceConfig
            {
                MethodConfigs = { methodConfig }
            }
        };

        var channel = GrpcChannel.ForAddress("https://localhost:5000", channelOptions);
        var client = new Product.ProductClient(channel);

        return client;
    }

    static async Task<ProductResponse> GetByIdAsync(Product.ProductClient client, int id)
    {
        var request = new ProductRequest { Id = id };

        var headers = new Metadata
        {
            { "Authorization", "Bearer your-token-here" },
            { "X-Api-Key", "335H3D8DD8J3D8D833SQ10" }
        };

        var options = new CallOptions(headers);

        try
        {
            var call = client.GetByIdAsync(request, options);

            var response = await call.ResponseAsync;

            if (call.GetStatus().StatusCode != StatusCode.OK)
            {
                return null;
            }

            var trailers = call.GetTrailers();
            if (trailers != null)
            {
                foreach (var entry in trailers)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }

            return response;
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Erro ao buscar produto: {ex.Status.Detail}");
            return null;
        }
    }

    static async Task<System.Collections.Generic.List<ProductResponse>> GetAllAsync(Product.ProductClient client)
    {
        var request = new Empty();

        var headers = new Metadata
        {
            { "Authorization", "Bearer your-token-here" },
            { "X-Api-Key", "335H3D8DD8J3D8D833SQ10" }
        };

        var options = new CallOptions(headers);

        try
        {
            var call = client.GetAllProductsAsync(request, options);

            var response = await call.ResponseAsync;

            if (call.GetStatus().StatusCode != StatusCode.OK)
            {
                return null;
            }

            var trailers = call.GetTrailers();
            if (trailers != null)
            {
                foreach (var entry in trailers)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }

            return new System.Collections.Generic.List<ProductResponse>(response.Products);
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Erro ao buscar produtos: {ex.Status.Detail}");
            return null;
        }
    }
}
