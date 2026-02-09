using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Queries;
using RedarborStore.Infrastructure.Data;

namespace RedarborStore.Infrastructure.Queries;

public class InventoryMovementQueryRepository : IInventoryMovementQueryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryMovementQueryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryMovement>> GetAllAsync()
    {
        return await _context.InventoryMovements
            .AsNoTracking()
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryMovement>> GetByProductAsync(int productId)
    {
        return await _context.InventoryMovements
            .AsNoTracking()
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync();
    }
}