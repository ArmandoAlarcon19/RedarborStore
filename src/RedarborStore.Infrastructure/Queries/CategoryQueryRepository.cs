using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Queries;
using RedarborStore.Infrastructure.Data;

namespace RedarborStore.Infrastructure.Queries;

public class CategoryQueryRepository : ICategoryQueryRepository
{
    private readonly InventoryDbContext _context;

    public CategoryQueryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Category> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Categories.AsNoTracking().CountAsync();

        var items = await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}