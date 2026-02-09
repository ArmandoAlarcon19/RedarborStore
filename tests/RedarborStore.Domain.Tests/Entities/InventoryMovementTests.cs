using FluentAssertions;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Tests.Builders;

namespace RedarborStore.Domain.Tests.Entities;

public class InventoryMovementTests
{

    [Fact]
    public void InventoryMovement_WhenCreated_ShouldHaveDefaultValues()
    {
        var movement = new InventoryMovement();
        movement.Id.Should().Be(0);
        movement.ProductId.Should().Be(0);
        movement.MovementType.Should().BeEmpty();
        movement.Quantity.Should().Be(0);
        movement.Reason.Should().BeNull();
        movement.MovementDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void InventoryMovement_WhenCreatedWithBuilder_ShouldHaveCorrectValues()
    {
        var movement = new InventoryMovementBuilder()
            .WithId(1)
            .WithProductId(5)
            .AsEntry()
            .WithQuantity(100)
            .WithReason("Initial stock")
            .Build();
        movement.Id.Should().Be(1);
        movement.ProductId.Should().Be(5);
        movement.MovementType.Should().Be("Entry");
        movement.Quantity.Should().Be(100);
        movement.Reason.Should().Be("Initial stock");
    }

    [Fact]
    public void InventoryMovement_AsEntry_ShouldHaveEntryType()
    {
        // Arrange & Act
        var movement = new InventoryMovementBuilder()
            .AsEntry()
            .Build();

        // Assert
        movement.MovementType.Should().Be("Entry");
    }

    [Fact]
    public void InventoryMovement_AsExit_ShouldHaveExitType()
    {
        var movement = new InventoryMovementBuilder()
            .AsExit()
            .Build();
        movement.MovementType.Should().Be("Exit");
    }

    [Theory]
    [InlineData("Entry")]
    [InlineData("Exit")]
    public void InventoryMovement_MovementType_ShouldAcceptValidTypes(string type)
    {
        var movement = new InventoryMovementBuilder()
            .WithMovementType(type)
            .Build();
        movement.MovementType.Should().Be(type);
    }

    [Theory]
    [InlineData("Invalid")]
    [InlineData("")]
    [InlineData("entry")]
    [InlineData("EXIT")]
    public void InventoryMovement_MovementType_EntityAllowsAnyString(string type)
    {
         var movement = new InventoryMovementBuilder()
            .WithMovementType(type)
            .Build();
        movement.MovementType.Should().Be(type);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(500)]
    [InlineData(10000)]
    public void InventoryMovement_Quantity_ShouldAcceptPositiveValues(int quantity)
    {
        var movement = new InventoryMovementBuilder()
            .WithQuantity(quantity)
            .Build();
        movement.Quantity.Should().Be(quantity);
        movement.Quantity.Should().BePositive();
    }

    [Fact]
    public void InventoryMovement_Reason_ShouldBeNullable()
    {
        var movement = new InventoryMovementBuilder()
            .WithReason(null)
            .Build();
        movement.Reason.Should().BeNull();
    }

    [Theory]
    [InlineData("Initial stock purchase")]
    [InlineData("Customer return")]
    [InlineData("Damaged goods removal")]
    [InlineData("Warehouse transfer")]
    public void InventoryMovement_Reason_ShouldAcceptValidReasons(string reason)
    {
        var movement = new InventoryMovementBuilder()
            .WithReason(reason)
            .Build();
        movement.Reason.Should().Be(reason);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(999)]
    public void InventoryMovement_ProductId_ShouldStoreCorrectValue(int productId)
    {
        var movement = new InventoryMovementBuilder()
            .WithProductId(productId)
            .Build();
        movement.ProductId.Should().Be(productId);
    }

    [Fact]
    public void EntryMovement_ShouldIncreaseProductStock()
    {
        var product = new ProductBuilder()
            .WithStock(100)
            .Build();

        var movement = new InventoryMovementBuilder()
            .AsEntry()
            .WithQuantity(50)
            .WithProductId(product.Id)
            .Build();
        if (movement.MovementType == "Entry")
        {
            product.Stock += movement.Quantity;
        }
        product.Stock.Should().Be(150);
    }

    [Fact]
    public void ExitMovement_ShouldDecreaseProductStock()
    {
        var product = new ProductBuilder()
            .WithStock(100)
            .Build();

        var movement = new InventoryMovementBuilder()
            .AsExit()
            .WithQuantity(30)
            .WithProductId(product.Id)
            .Build();
        if (movement.MovementType == "Exit")
        {
            product.Stock -= movement.Quantity;
        }
        product.Stock.Should().Be(70);
    }

    [Fact]
    public void ExitMovement_WithExactStock_ShouldResultInZero()
    {
        var product = new ProductBuilder()
            .WithStock(50)
            .Build();
        var movement = new InventoryMovementBuilder()
            .AsExit()
            .WithQuantity(50)
            .Build();
        product.Stock -= movement.Quantity;
        product.Stock.Should().Be(0);
    }

    [Fact]
    public void MultipleMovements_ShouldCalculateStockCorrectly()
    {
        var product = new ProductBuilder()
            .WithStock(100)
            .Build();
        var movements = new List<InventoryMovement>
        {
            new InventoryMovementBuilder().AsEntry().WithQuantity(50).Build(),   // +50 = 150
            new InventoryMovementBuilder().AsExit().WithQuantity(30).Build(),    // -30 = 120
            new InventoryMovementBuilder().AsEntry().WithQuantity(20).Build(),   // +20 = 140
            new InventoryMovementBuilder().AsExit().WithQuantity(10).Build(),    // -10 = 130
        };
        foreach (var movement in movements)
        {
            if (movement.MovementType == "Entry")
                product.Stock += movement.Quantity;
            else
                product.Stock -= movement.Quantity;
        }
        product.Stock.Should().Be(130);
    }

    [Fact]
    public void ExitMovement_ExceedingStock_ShouldResultInNegative()
    {
        var product = new ProductBuilder()
            .WithStock(10)
            .Build();
        product.Stock -= 20;
        product.Stock.Should().BeNegative();
        product.Stock.Should().Be(-10);
    }
}