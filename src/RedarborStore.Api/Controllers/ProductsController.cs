using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedarborStore.Application.Features.Products.Commands.CreateProduct;
using RedarborStore.Application.Features.Products.Commands.DeleteProduct;
using RedarborStore.Application.Features.Products.Commands.UpdateProduct;
using RedarborStore.Application.Features.Products.Queries.GetAllProducts;
using RedarborStore.Application.Features.Products.Queries.GetProductById;
using RedarborStore.Application.Features.Products.Queries.GetProductsByCategory;

namespace RedarborStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        if (result == null) return NotFound("Product not found");
        return Ok(result);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _mediator.Send(new GetProductsByCategoryQuery { CategoryId = categoryId });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        if (!result) return NotFound("Product not found");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        if (!result) return NotFound("Product not found");
        return NoContent();
    }
}