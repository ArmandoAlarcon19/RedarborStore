using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetInventoryMovementsByProduct;

namespace RedarborStore.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class InventoryMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllInventoryMovementsQuery());
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var result = await _mediator.Send(
            new GetInventoryMovementsByProductQuery { ProductId = productId });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInventoryMovementCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id, Message = "Movement registered successfully" });
       
    }
}