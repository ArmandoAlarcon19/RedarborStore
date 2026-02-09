using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Commands.DeleteProduct;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Products.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly IProductCommandRepository _commandRepository;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _commandRepository = Substitute.For<IProductCommandRepository>();
        _handler = new DeleteProductCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_ShouldReturnTrue()
    {
        var command = new DeleteProductCommand { Id = 1 };
        _commandRepository.DeleteAsync(1).Returns(true);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ShouldReturnFalse()
    {
        var command = new DeleteProductCommand { Id = 999 };
        _commandRepository.DeleteAsync(999).Returns(false);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteWithCorrectId()
    {
        var command = new DeleteProductCommand { Id = 7 };
        _commandRepository.DeleteAsync(7).Returns(true);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository.Received(1).DeleteAsync(7);
    }
}