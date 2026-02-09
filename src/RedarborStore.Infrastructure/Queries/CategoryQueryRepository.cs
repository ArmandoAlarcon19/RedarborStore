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

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}