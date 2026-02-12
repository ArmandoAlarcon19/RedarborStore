using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedarborStore.Application.Features.Categories.Commands.CreateCategory;
using RedarborStore.Application.Features.Categories.Commands.DeleteCategory;
using RedarborStore.Application.Features.Categories.Commands.UpdateCategory;
using RedarborStore.Application.Features.Categories.Queries.GetAllCategories;
using RedarborStore.Application.Features.Categories.Queries.GetCategoryById;

namespace RedarborStore.Api.Controllers;

/// <summary>
/// Gestión de categorías del inventario
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las categorías con paginación
    /// </summary>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10, max: 50)</param>
    /// <returns>Lista paginada de categorías</returns>
    /// <response code="200">Retorna la lista paginada de categorías</response>
    /// <response code="401">No autorizado</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 50) pageSize = 50;
        if (pageNumber < 1) pageNumber = 1;

        var result = await _mediator.Send(new GetAllCategoriesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    /// <param name="id">ID de la categoría</param>
    /// <returns>Detalle de la categoría</returns>
    /// <response code="200">Retorna la categoría solicitada</response>
    /// <response code="404">Categoría no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    /// <param name="command">Datos de la nueva categoría</param>
    /// <returns>ID de la categoría creada</returns>
    /// <response code="201">Categoría creada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    /// <param name="id">ID de la categoría a actualizar</param>
    /// <param name="command">Datos actualizados</param>
    /// <response code="204">Categoría actualizada exitosamente</response>
    /// <response code="404">Categoría no encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Elimina una categoría (soft delete)
    /// </summary>
    /// <param name="id">ID de la categoría a eliminar</param>
    /// <response code="204">Categoría eliminada exitosamente</response>
    /// <response code="404">Categoría no encontrada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}