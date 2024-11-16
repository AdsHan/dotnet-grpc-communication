using GrpcCommunication.API.Application.DTO;
using GrpcCommunication.API.Data;
using GrpcCommunication.Grpc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrpcCommunication.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public ProductsController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/products
    /// <summary>
    /// Obtêm os produtos
    /// </summary>
    /// <returns>Coleção de objetos da classe Produto</returns>                
    /// <response code="200">Lista dos produtos</response>        
    /// <response code="400">Falha na requisição</response>         
    /// <response code="404">Nenhum produto foi localizado</response>         
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .Select(p => new ProductDTO(
                p.Id,
                p.Title,
                p.Description,
                p.Price,
                p.Quantity,
                "API 1"
            )).ToListAsync();

        return products.Any() ? Ok(products) : NotFound();
    }


    // GET: api/products/5
    /// <summary>
    /// Obtêm as informações do produto pelo seu Id
    /// </summary>
    /// <param name="id">Código do produto</param>
    /// <returns>Objetos da classe Produto</returns>                
    /// <response code="200">Informações do Producto</response>
    /// <response code="400">Falha na requisição</response>         
    /// <response code="404">O produto não foi localizado</response>         
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                Origin = "API 1"
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        return product != null ? Ok(product) : NotFound();
    }
}
