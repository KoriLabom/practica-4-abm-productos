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

    [HttpGet("search")]
    public IActionResult SearchProductsByName([FromQuery] string name)
    {
        List<ProductForReadDto> products = _service.SearchProductsByName(name);
        return Ok(products);
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        ProductStatsDto stats = _service.GetStats();
        return Ok(stats);
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductForCreateDto dto)
    {
        try
        {
            ProductForReadDto created = _service.CreateProduct(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, ProductForUpdateDto dto)
    {
        if (_service.GetProductById(id) == null)
        {
            return NotFound();
        }
        try
        {
            _service.UpdateProduct(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
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
