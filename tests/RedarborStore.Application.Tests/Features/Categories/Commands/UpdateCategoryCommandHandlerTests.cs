using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Categories.Commands.UpdateCategory;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Categories.Commands;

public class UpdateCategoryCommandHandlerTests
{
    private readonly ICategoryCommandRepository _commandRepository;
    private readonly UpdateCategoryCommandHandler _handler;

    public UpdateCategoryCommandHandlerTests()
    {
        _commandRepository = Substitute.For<ICategoryCommandRepository>();
        _handler = new UpdateCategoryCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnTrue()
    {
        var command = new UpdateCategoryCommand
        {
            Id = 1,
            Name = "Updated Electronics",
            Description = "Updated description"
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Category>())
            .Returns(true);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnFalse()
    {
        var command = new UpdateCategoryCommand
        {
            Id = 999,
            Name = "Non existent",
            Description = null
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Category>())
            .Returns(false);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectDataToRepository()
    {
        var command = new UpdateCategoryCommand
        {
            Id = 3,
            Name = "Food",
            Description = "Updated food category"
        };
        _commandRepository
            .UpdateAsync(Arg.Any<Category>())
            .Returns(true);

        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Category>(c =>
                c.Id == 3 &&
                c.Name == "Food" &&
                c.Description == "Updated food category"
            ));
    }
}