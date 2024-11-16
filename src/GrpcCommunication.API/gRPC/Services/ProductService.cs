using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcCommunication.API.Data;
using GrpcCommunication.API.Data.Enums;
using GrpcCommunication.Grpc;
using Microsoft.EntityFrameworkCore;

namespace GrpcCommunication.API.gRPC.Services;

public class ProductService : Product.ProductBase
{
    private readonly CatalogDbContext _dbContext;

    public ProductService(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ProductResponse> GetById(ProductRequest request, ServerCallContext context)
    {
        try
        {
            var product = await _dbContext.Products
            .AsNoTracking()
            .Where(a => a.Status == EntityStatusEnum.Active && a.Id == request.Id)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                Origin = "API 1"
            })
            .FirstOrDefaultAsync();

            context.ResponseTrailers.Add("X-Processing-Time", "123ms");
            context.ResponseTrailers.Add("X-Status", "Success");

            return product ?? new ProductResponse();
        }
        catch (Exception ex)
        {
            context.ResponseTrailers.Add("X-Error", ex.Message);
            throw;
        }
    }

    public override async Task<ProductListResponse> GetAllProducts(Empty request, ServerCallContext context)
    {
        try
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Where(a => a.Status == EntityStatusEnum.Active)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Origin = "API 1"
                }
                ).ToListAsync();

            context.ResponseTrailers.Add("X-Processing-Time", "123ms");
            context.ResponseTrailers.Add("X-Status", "Success");

            return new ProductListResponse
            {
                Products = { products }
            };

        }
        catch (Exception ex)
        {
            context.ResponseTrailers.Add("X-Error", ex.Message);
            throw;
        }
    }
}
