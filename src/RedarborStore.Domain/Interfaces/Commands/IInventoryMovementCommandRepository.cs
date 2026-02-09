using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Commands;

public interface IInventoryMovementCommandRepository
{
    Task<int> CreateAsync(InventoryMovement movement);
}