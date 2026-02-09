using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Categories.Commands.DeleteCategory;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Categories.Commands;

public class DeleteCategoryCommandHandlerTests
{
    private readonly ICategoryCommandRepository _commandRepository;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _commandRepository = Substitute.For<ICategoryCommandRepository>();
        _handler = new DeleteCategoryCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithExistingId_ShouldReturnTrue()
    {
        var command = new DeleteCategoryCommand { Id = 1 };
        _commandRepository
            .DeleteAsync(1)
            .Returns(true);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnFalse()
    {
        var command = new DeleteCategoryCommand { Id = 999 };
        _commandRepository
            .DeleteAsync(999)
            .Returns(false);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        var command = new DeleteCategoryCommand { Id = 5 };
        _commandRepository
            .DeleteAsync(5)
            .Returns(true);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .DeleteAsync(5);
    }
}