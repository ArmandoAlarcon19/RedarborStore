using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Commands.UpdateProduct;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly IProductCommandRepository _commandRepository;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTests()
    {
        _commandRepository = Substitute.For<IProductCommandRepository>();
        _handler = new UpdateProductCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnTrue()
    {
        var command = new UpdateProductCommand
        {
            Id = 1,
            Name = "Updated Laptop",
            Description = "Updated desc",
            Price = 1499.99m,
            CategoryId = 1
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Product>())
            .Returns(true);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ShouldReturnFalse()
    {
        var command = new UpdateProductCommand
        {
            Id = 999,
            Name = "Ghost Product",
            Price = 10m,
            CategoryId = 1
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Product>())
            .Returns(false);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldSetUpdatedAtToCurrentTime()
    {
        var command = new UpdateProductCommand
        {
            Id = 1,
            Name = "Updated",
            Price = 100m,
            CategoryId = 1
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Product>())
            .Returns(true);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Product>(p =>
                p.UpdatedDate != null &&
                p.UpdatedDate.Value <= DateTime.Now.AddSeconds(5)));
    }
}