using Microsoft.AspNetCore.Mvc;
using ToyStore.API.Interfaces;
using ToyStore.API.Models;

namespace ToyStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _repository.GetById(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        product.Id = 0;

        _repository.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        product.Id = id;

        if (!_repository.Update(product))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_repository.Delete(id))
            return NotFound();

        return NoContent();
    }
}