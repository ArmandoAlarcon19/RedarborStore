using Domain.Entities;

namespace Domain.Interfaces.Commands;

public interface IInventoryMovementCommandRepository
{
    Task<int> CreateAsync(InventoryMovement movement);
}