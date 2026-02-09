using Domain.Entities;
using Domain.Interfaces.Queries;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries;

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