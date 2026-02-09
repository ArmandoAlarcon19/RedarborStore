using MediatR;

namespace RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;

public class CreateInventoryMovementCommand : IRequest<int>
{
    public int ProductId { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Reason { get; set; }
}