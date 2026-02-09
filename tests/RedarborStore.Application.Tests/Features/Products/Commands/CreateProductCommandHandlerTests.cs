using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Commands.CreateProduct;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly IProductCommandRepository _commandRepository;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _commandRepository = Substitute.For<IProductCommandRepository>();
        _handler = new CreateProductCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnNewId()
    {
        var command = new CreateProductCommand
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1299.99m,
            Stock = 50,
            CategoryId = 1
        };
        _commandRepository
            .CreateAsync(Arg.Any<Product>())
            .Returns(1);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldMapAllPropertiesToEntity()
    {
        var command = new CreateProductCommand
        {
            Name = "Keyboard",
            Description = "Mechanical RGB",
            Price = 149.99m,
            Stock = 75,
            CategoryId = 1
        };

        _commandRepository
            .CreateAsync(Arg.Any<Product>())
            .Returns(3);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .CreateAsync(Arg.Is<Product>(p =>
                p.Name == "Keyboard" &&
                p.Description == "Mechanical RGB" &&
                p.Price == 149.99m &&
                p.Stock == 75 &&
                p.CategoryId == 1));
    }

    [Fact]
    public async Task Handle_WithZeroStock_ShouldStillCreate()
    {
        var command = new CreateProductCommand
        {
            Name = "Coming Soon Product",
            Description = "Pre-order",
            Price = 49.99m,
            Stock = 0,
            CategoryId = 2
        };
        _commandRepository
            .CreateAsync(Arg.Any<Product>())
            .Returns(5);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(5);
        await _commandRepository
            .Received(1)
            .CreateAsync(Arg.Is<Product>(p => p.Stock == 0));
    }
}