using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Queries;

public interface IInventoryMovementQueryRepository
{
    Task<IEnumerable<InventoryMovement>> GetAllAsync();
    Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId);
}