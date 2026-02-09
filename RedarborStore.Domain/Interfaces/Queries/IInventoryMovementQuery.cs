using Domain.Entities;

namespace Domain.Interfaces.Queries;

public interface IInventoryMovementQueryRepository
{
    Task<IEnumerable<InventoryMovement>> GetAllAsync();
    Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId);
}