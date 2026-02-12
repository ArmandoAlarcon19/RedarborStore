using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Queries;

public interface ICategoryQueryRepository
{
    Task<(IEnumerable<Category> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);    
    Task<Category?> GetByIdAsync(int id);
}