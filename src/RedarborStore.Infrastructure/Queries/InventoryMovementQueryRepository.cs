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

      public async Task<(IEnumerable<InventoryMovement> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.InventoryMovements.AsNoTracking().CountAsync();

        var items = await _context.InventoryMovements
            .AsNoTracking()
            .OrderByDescending(m => m.MovementDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
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