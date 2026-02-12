using FluentAssertions;
using RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;
using RedarborStore.Domain.Enums;

namespace RedarborStore.Application.Tests.Features.InventoryMovements.Commands;

public class CreateInventoryMovementCommandValidatorTests
{
    private readonly CreateInventoryMovementCommandValidator _validator;

    public CreateInventoryMovementCommandValidatorTests()
    {
        _validator = new CreateInventoryMovementCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidEntryCommand_ShouldPass()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.Entry,
            Quantity = 50,
            Reason = "Restock"
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithValidExitCommand_ShouldPass()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.Exit,
            Quantity = 10,
            Reason = "Sale"
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithNoneMovementType_ShouldFail()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.None,
            Quantity = 10,
            Reason = "Test"
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MovementType");
    }

    [Fact]
    public async Task Validate_WithZeroQuantity_ShouldFail()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.Entry,
            Quantity = 0,
            Reason = "Test"
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
    }

    [Fact]
    public async Task Validate_WithZeroProductId_ShouldFail()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 0,
            MovementType = MovementType.Entry,
            Quantity = 10,
            Reason = "Test"
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
    }

    [Fact]
    public async Task Validate_WithReasonExceeding250Chars_ShouldFail()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.Entry,
            Quantity = 10,
            Reason = new string('R', 251)
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Reason");
    }

    [Fact]
    public async Task Validate_WithNullReason_ShouldPass()
    {
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = MovementType.Entry,
            Quantity = 10,
            Reason = null
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}