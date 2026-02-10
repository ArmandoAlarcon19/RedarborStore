using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.InventoryMovements.Commands;

public class CreateInventoryMovementCommandHandlerTests
{
    private readonly IInventoryMovementCommandRepository _movementCommandRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IProductCommandRepository _productCommandRepository;
    private readonly CreateInventoryMovementCommandHandler _handler;

    public CreateInventoryMovementCommandHandlerTests()
    {
        _movementCommandRepository = Substitute.For<IInventoryMovementCommandRepository>();
        _productQueryRepository = Substitute.For<IProductQueryRepository>();
        _productCommandRepository = Substitute.For<IProductCommandRepository>();

        _handler = new CreateInventoryMovementCommandHandler(
            _movementCommandRepository,
            _productQueryRepository,
            _productCommandRepository);
    }
    [Fact]
    public async Task Handle_EntryMovement_ShouldIncreaseStockAndReturnId()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 100);
        _productQueryRepository
            .GetByIdAsync(1)
            .Returns(product);
        _productCommandRepository
            .UpdateStockAsync(1, 150)
            .Returns(true);
        _movementCommandRepository
            .CreateAsync(Arg.Any<InventoryMovement>())
            .Returns(1);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Entry",
            Quantity = 50,
            Reason = "Restock"
        };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(1);
        await _productCommandRepository.Received(1).UpdateStockAsync(1, 150);
    }

    [Fact]
    public async Task Handle_EntryMovement_ShouldCallUpdateStockWithCorrectNewStock()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 200);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        _productCommandRepository.UpdateStockAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
        _movementCommandRepository.CreateAsync(Arg.Any<InventoryMovement>()).Returns(5);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Entry",
            Quantity = 100,
            Reason = "Large shipment"
        };
        await _handler.Handle(command, CancellationToken.None);
        await _productCommandRepository.Received(1).UpdateStockAsync(1, 300);
    }

    [Fact]
    public async Task Handle_ExitMovement_ShouldDecreaseStockAndReturnId()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 100);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        _productCommandRepository.UpdateStockAsync(1, 70).Returns(true);
        _movementCommandRepository.CreateAsync(Arg.Any<InventoryMovement>()).Returns(2);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Exit",
            Quantity = 30,
            Reason = "Customer order"
        };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(2);
        await _productCommandRepository.Received(1).UpdateStockAsync(1, 70);
    }

    [Fact]
    public async Task Handle_ExitMovement_WithExactStock_ShouldSetStockToZero()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 50);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        _productCommandRepository.UpdateStockAsync(1, 0).Returns(true);
        _movementCommandRepository.CreateAsync(Arg.Any<InventoryMovement>()).Returns(3);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Exit",
            Quantity = 50,
            Reason = "Full inventory clearance"
        };
        await _handler.Handle(command, CancellationToken.None);
        await _productCommandRepository.Received(1).UpdateStockAsync(1, 0);
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ShouldThrowException()
    {
        _productQueryRepository
            .GetByIdAsync(999)
            .Returns((Product?)null);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 999,
            MovementType = "Entry",
            Quantity = 10,
            Reason = "Test"
        };
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*999*not found*");
    }

    [Fact]
    public async Task Handle_WithInvalidMovementType_ShouldThrowException()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 100);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "InvalidType",
            Quantity = 10,
            Reason = "Test"
        };
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*Entry*Exit*");
    }

    [Fact]
    public async Task Handle_ExitWithInsufficientStock_ShouldThrowException()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 10);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Exit",
            Quantity = 50,
            Reason = "Large order"
        };
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("*Insufficient stock*10*");
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldNotCallAnyCommandRepository()
    {
        _productQueryRepository
            .GetByIdAsync(999)
            .Returns((Product?)null);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 999,
            MovementType = "Entry",
            Quantity = 10
        };
        try { await _handler.Handle(command, CancellationToken.None); }
        catch { /* Expected */ }
        await _productCommandRepository.DidNotReceive().UpdateStockAsync(Arg.Any<int>(), Arg.Any<int>());
        await _movementCommandRepository.DidNotReceive().CreateAsync(Arg.Any<InventoryMovement>());
    }

    [Fact]
    public async Task Handle_WhenInvalidType_ShouldNotCallAnyCommandRepository()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 100);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "BadType",
            Quantity = 10
        };
        try { await _handler.Handle(command, CancellationToken.None); }
        catch { /* Expected */ }
        await _productCommandRepository.DidNotReceive().UpdateStockAsync(Arg.Any<int>(), Arg.Any<int>());
        await _movementCommandRepository.DidNotReceive().CreateAsync(Arg.Any<InventoryMovement>());
    }

    [Fact]
    public async Task Handle_SuccessfulMovement_ShouldCreateMovementWithCorrectData()
    {
        var product = ProductFixture.CreateProduct(id: 1, stock: 100);
        _productQueryRepository.GetByIdAsync(1).Returns(product);
        _productCommandRepository.UpdateStockAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
        _movementCommandRepository.CreateAsync(Arg.Any<InventoryMovement>()).Returns(1);

        var command = new CreateInventoryMovementCommand
        {
            ProductId = 1,
            MovementType = "Entry",
            Quantity = 25,
            Reason = "Weekly restock"
        };
        await _handler.Handle(command, CancellationToken.None);
        await _movementCommandRepository
            .Received(1)
            .CreateAsync(Arg.Is<InventoryMovement>(m =>
                m.ProductId == 1 &&
                m.MovementType == "Entry" &&
                m.Quantity == 25 &&
                m.Reason == "Weekly restock"));
    }
}