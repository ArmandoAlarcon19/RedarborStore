using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Queries;

public interface IInventoryMovementQueryRepository
{
    Task<(IEnumerable<InventoryMovement> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId);
}