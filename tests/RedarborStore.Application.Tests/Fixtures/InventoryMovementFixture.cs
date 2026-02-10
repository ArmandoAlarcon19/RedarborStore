using RedarborStore.Domain.Entities;

namespace RedarborStore.Application.Tests.Fixtures;

public static class InventoryMovementFixture
{
    public static InventoryMovement CreateMovement(
        int id = 1,
        int productId = 1,
        string movementType = "Entry",
        int quantity = 50,
        string? reason = "Stock replenishment")
    {
        return new InventoryMovement
        {
            Id = id,
            ProductId = productId,
            MovementType = movementType,
            Quantity = quantity,
            Reason = reason,
            MovementDate = new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc)
        };
    }

    public static List<InventoryMovement> CreateMovementList()
    {
        return new List<InventoryMovement>
        {
            CreateMovement(1, 1, "Entry", 100, "Initial stock"),
            CreateMovement(2, 1, "Exit", 20, "Customer order #1001"),
            CreateMovement(3, 1, "Entry", 50, "Restock from supplier"),
            CreateMovement(4, 2, "Entry", 200, "New product arrival"),
        };
    }

    public static List<InventoryMovement> CreateMovementListByProduct(int productId)
    {
        return CreateMovementList()
            .Where(m => m.ProductId == productId)
            .ToList();
    }

    public static List<InventoryMovement> CreateEmptyList() => new List<InventoryMovement>();
}