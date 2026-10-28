using AbmProductos.Entities;
using AbmProductos.Models.DTOs.Requests;
using AbmProductos.Models.DTOs.Responses;
using AbmProductos.Repositories.Interfaces;
using AbmProductos.Services.Interfaces;

namespace AbmProductos.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductForReadDto> GetAllProducts()
    {
        List<ProductForReadDto> result = new List<ProductForReadDto>();
        foreach (Product product in _repository.GetAllProducts())
        {
            result.Add(MapToReadDto(product));
        }
        return result;
    }

    public ProductForReadDto? GetProductById(int id)
    {
        Product? product = _repository.GetProductById(id);
        if (product == null)
        {
            return null;
        }
        return MapToReadDto(product);
    }

    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        if (ExistsProductWithName(dto.Name, 0))
        {
            throw new InvalidOperationException("Ya existe un producto con el nombre '" + dto.Name + "'.");
        }

        Product product = new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };
        _repository.AddProduct(product);
        return MapToReadDto(product);
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        Product? product = _repository.GetProductById(id);
        if (product == null)
        {
            return;
        }
        if (ExistsProductWithName(dto.Name, id))
        {
            throw new InvalidOperationException("Ya existe un producto con el nombre '" + dto.Name + "'.");
        }
        product.Name = dto.Name;
        product.Price = dto.Price;
        _repository.UpdateProduct(product);
    }

    public void DeleteProduct(int id)
    {
        Product? product = _repository.GetProductById(id);
        if (product == null)
        {
            return;
        }
        _repository.DeleteProduct(product);
    }

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        List<ProductForReadDto> result = new List<ProductForReadDto>();
        foreach (Product product in _repository.SearchProductsByName(name))
        {
            result.Add(MapToReadDto(product));
        }
        return result;
    }

    public ProductStatsDto GetStats()
    {
        List<Product> products = _repository.GetAllProducts();
        if (products.Count == 0)
        {
            return new ProductStatsDto();
        }

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = Math.Round(products.Average(p => p.Price), 2),
            MostExpensiveName = products.OrderByDescending(p => p.Price).First().Name
        };
    }

    private bool ExistsProductWithName(string name, int excludedId)
    {
        return _repository.GetAllProducts().Any(p => p.Id != excludedId && p.Name.ToLower() == name.ToLower());
    }

    private static ProductForReadDto MapToReadDto(Product product)
    {
        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
