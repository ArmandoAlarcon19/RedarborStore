using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Infrastructure.Commands;

public class CategoryCommandRepository : ICategoryCommandRepository
{
    private readonly string _connectionString;

    public CategoryCommandRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string DefaultConnection not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<int> CreateAsync(Category category)
    {
        const string sql = @"
            INSERT INTO Categories (Name, Description, CreatedDate)
            VALUES (@Name, @Description, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        using var connection = CreateConnection();
        
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            category.Name,
            category.Description
        });
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        const string sql = @"
            UPDATE Categories
            SET Name        = @Name,
                Description = @Description
            WHERE Id = @Id AND IsDeleted = 0;";
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            category.Id,
            category.Name,
            category.Description
        });

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
            UPDATE Categories
            SET IsDeleted = 1, DeletedDate = GETDATE()
            WHERE Id = @Id;";
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        
        return rowsAffected > 0;
    }
}