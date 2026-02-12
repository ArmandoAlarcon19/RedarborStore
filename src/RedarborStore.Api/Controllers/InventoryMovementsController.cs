using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetInventoryMovementsByProduct;

namespace RedarborStore.Api.Controllers;

/// <summary>
/// Gestión de movimientos de inventario (entradas y salidas)
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
[Produces("application/json")]
public class InventoryMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los movimientos de inventario con paginación
    /// </summary>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10, max: 50)</param>
    /// <returns>Lista paginada de movimientos</returns>
    /// <response code="200">Retorna la lista paginada de movimientos</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 50) pageSize = 50;
        if (pageNumber < 1) pageNumber = 1;

        var result = await _mediator.Send(new GetAllInventoryMovementsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Obtiene movimientos de inventario filtrados por producto
    /// </summary>
    /// <param name="productId">ID del producto</param>
    /// <returns>Lista de movimientos del producto</returns>
    /// <response code="200">Retorna los movimientos del producto</response>
    [HttpGet("product/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var result = await _mediator.Send(
            new GetInventoryMovementsByProductQuery { ProductId = productId });
        return Ok(result);
    }

    /// <summary>
    /// Registra un nuevo movimiento de inventario (entrada o salida)
    /// </summary>
    /// <param name="command">Datos del movimiento</param>
    /// <returns>ID del movimiento registrado</returns>
    /// <response code="200">Movimiento registrado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="422">Regla de negocio violada (stock insuficiente)</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateInventoryMovementCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id, Message = "Movement registered successfully" });
    }
}