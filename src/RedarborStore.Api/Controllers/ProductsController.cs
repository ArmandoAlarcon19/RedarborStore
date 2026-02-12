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

/// <summary>
/// Gestión de productos del inventario
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los productos con paginación
    /// </summary>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10, max: 50)</param>
    /// <returns>Lista paginada de productos</returns>
    /// <response code="200">Retorna la lista paginada de productos</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 50) pageSize = 50;
        if (pageNumber < 1) pageNumber = 1;

        var result = await _mediator.Send(new GetAllProductsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Detalle del producto</returns>
    /// <response code="200">Retorna el producto solicitado</response>
    /// <response code="404">Producto no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Obtiene productos filtrados por categoría
    /// </summary>
    /// <param name="categoryId">ID de la categoría</param>
    /// <returns>Lista de productos de la categoría</returns>
    /// <response code="200">Retorna los productos de la categoría</response>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _mediator.Send(new GetProductsByCategoryQuery { CategoryId = categoryId });
        return Ok(result);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="command">Datos del nuevo producto</param>
    /// <returns>ID del producto creado</returns>
    /// <response code="201">Producto creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto a actualizar</param>
    /// <param name="command">Datos actualizados</param>
    /// <response code="204">Producto actualizado exitosamente</response>
    /// <response code="404">Producto no encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Elimina un producto (soft delete)
    /// </summary>
    /// <param name="id">ID del producto a eliminar</param>
    /// <response code="204">Producto eliminado exitosamente</response>
    /// <response code="404">Producto no encontrado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }
}