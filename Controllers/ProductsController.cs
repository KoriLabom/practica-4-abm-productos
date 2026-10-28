using AbmProductos.Models.DTOs.Requests;
using AbmProductos.Models.DTOs.Responses;
using AbmProductos.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace AbmProductos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private ProductService _service = new ProductService();

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        List<ProductForReadDto> products = _service.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        ProductForReadDto? product = _service.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductForCreateDto dto)
    {
        ProductForReadDto created = _service.CreateProduct(dto);
        return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, ProductForUpdateDto dto)
    {
        if (_service.GetProductById(id) == null)
        {
            return NotFound();
        }
        _service.UpdateProduct(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        if (_service.GetProductById(id) == null)
        {
            return NotFound();
        }
        _service.DeleteProduct(id);
        return NoContent();
    }
}
