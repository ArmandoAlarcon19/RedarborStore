using RedarborStore.Domain.Entities;

namespace RedarborStore.Domain.Tests.Builders;

public class InventoryMovementBuilder
{
    private int _id = 1;
    private int _productId = 1;
    private string _movementType = "Entry";
    private int _quantity = 50;
    private string? _reason = "Test movement";
    private DateTime _movementDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public InventoryMovementBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public InventoryMovementBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public InventoryMovementBuilder AsEntry()
    {
        _movementType = "Entry";
        return this;
    }

    public InventoryMovementBuilder AsExit()
    {
        _movementType = "Exit";
        return this;
    }

    public InventoryMovementBuilder WithMovementType(string type)
    {
        _movementType = type;
        return this;
    }

    public InventoryMovementBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public InventoryMovementBuilder WithReason(string? reason)
    {
        _reason = reason;
        return this;
    }

    public InventoryMovement Build()
    {
        return new InventoryMovement
        {
            Id = _id,
            ProductId = _productId,
            MovementType = _movementType,
            Quantity = _quantity,
            Reason = _reason,
            MovementDate = _movementDate
        };
    }
}