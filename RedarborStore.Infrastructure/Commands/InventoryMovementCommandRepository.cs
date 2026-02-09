using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Commands;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Commands;

public class InventoryMovementCommandRepository : IInventoryMovementCommandRepository
{
    private readonly string _connectionString;

    public InventoryMovementCommandRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<int> CreateAsync(InventoryMovement movement)
    {
        const string sql = @"
            INSERT INTO InventoryMovements (ProductId, MovementType, Quantity, Reason, MovementDate)
            VALUES (@ProductId, @MovementType, @Quantity, @Reason, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            movement.ProductId,
            movement.MovementType,
            movement.Quantity,
            movement.Reason
        });
    }
}