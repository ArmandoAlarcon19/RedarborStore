using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Infrastructure.Commands;

public class ProductCommandRepository : IProductCommandRepository
{
    private readonly string _connectionString;

    public ProductCommandRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<int> CreateAsync(Product product)
    {
        const string sql = @"
            INSERT INTO Products (Name, Description, Price, Stock, CategoryId, CreatedDate)
            VALUES (@Name, @Description, @Price, @Stock, @CategoryId, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var connection = CreateConnection();
        
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.CategoryId
        });
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        const string sql = @"
            UPDATE Products
            SET Name        = @Name,
                Description = @Description,
                Price       = @Price,
                CategoryId  = @CategoryId,
                UpdatedDate   = GETDATE()
            WHERE Id = @Id AND IsDeleted = 0;";
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CategoryId
        });

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
            UPDATE Products
            SET IsDeleted = 1, DeletedDate = GETDATE()
            WHERE Id = @Id;";
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStockAsync(int productId, int newStock)
    {
        const string sql = @"
            UPDATE Products
            SET Stock     = @NewStock,
                UpdatedDate = GETDATE()
            WHERE Id = @ProductId AND IsDeleted = 0;";
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            ProductId = productId,
            NewStock = newStock
        });

        return rowsAffected > 0;
    }
}