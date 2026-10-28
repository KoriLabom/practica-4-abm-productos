using AbmProductos.Entities;
using AbmProductos.Models.DTOs.Requests;
using AbmProductos.Models.DTOs.Responses;
using AbmProductos.Repositories.Implementations;
using AbmProductos.Services.Interfaces;

namespace AbmProductos.Services.Implementations;

public class ProductService : IProductService
{
    private ProductRepository _repository = new ProductRepository();

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
