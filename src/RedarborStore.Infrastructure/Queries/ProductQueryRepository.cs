using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Queries;
using RedarborStore.Infrastructure.Data;

namespace RedarborStore.Infrastructure.Queries;

public class ProductQueryRepository : IProductQueryRepository
{
    private readonly InventoryDbContext _context;

    public ProductQueryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}