using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Queries;

public interface ICategoryQueryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}