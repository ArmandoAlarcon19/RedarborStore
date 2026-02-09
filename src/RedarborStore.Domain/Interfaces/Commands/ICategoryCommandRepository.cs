using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Interfaces.Commands;

public interface ICategoryCommandRepository
{
    Task<int> CreateAsync(Category category);
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
}