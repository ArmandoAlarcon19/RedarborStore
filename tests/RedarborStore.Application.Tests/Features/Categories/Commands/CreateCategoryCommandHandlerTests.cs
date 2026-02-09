using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Categories.Commands.CreateCategory;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Tests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly ICategoryCommandRepository _commandRepository;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _commandRepository = Substitute.For<ICategoryCommandRepository>();
        _handler = new CreateCategoryCommandHandler(_commandRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnNewId()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Electronics",
            Description = "Electronic devices"
        };
        _commandRepository
            .CreateAsync(Arg.Any<Category>())
            .Returns(1);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectData()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Clothing",
            Description = "Apparel items"
        };
        _commandRepository
            .CreateAsync(Arg.Any<Category>())
            .Returns(5);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .CreateAsync(Arg.Is<Category>(c =>
                c.Name == "Clothing" &&
                c.Description == "Apparel items"));
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryExactlyOnce()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Food",
            Description = null
        };
        _commandRepository
            .CreateAsync(Arg.Any<Category>())
            .Returns(1);
        await _handler.Handle(command, CancellationToken.None);
        await _commandRepository
            .Received(1)
            .CreateAsync(Arg.Any<Category>());
    }

    [Fact]
    public async Task Handle_WithNullDescription_ShouldStillCreateCategory()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Misc",
            Description = null
        };
        _commandRepository
            .CreateAsync(Arg.Any<Category>())
            .Returns(10);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(10);
        await _commandRepository
            .Received(1)
            .CreateAsync(Arg.Is<Category>(c =>
                c.Name == "Misc" &&
                c.Description == null));
    }
}