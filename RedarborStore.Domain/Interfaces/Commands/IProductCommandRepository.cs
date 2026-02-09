using Domain.Entities;

namespace Domain.Interfaces.Commands;

public interface IProductCommandRepository
{
    Task<int> CreateAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int productId, int newStock);
}