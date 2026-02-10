using MediatR;
using RedarborStore.Domain.Enums;

namespace RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;

public class CreateInventoryMovementCommand : IRequest<int>
{
    public int ProductId { get; set; }
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public string? Reason { get; set; }
}