using Domain.Entities;

namespace Domain.Interfaces.Queries;

public interface ICategoryQueryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}