using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Categories.Queries.GetCategoryById;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly ICategoryQueryRepository _queryRepository;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        _queryRepository = Substitute.For<ICategoryQueryRepository>();
        _handler = new GetCategoryByIdQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingId_ShouldReturnCategory()
    {
        var category = CategoryFixture.CreateCategory(1, "Electronics", "Devices");
        _queryRepository
            .GetByIdAsync(1)
            .Returns(category);
        var query = new GetCategoryByIdQuery { Id = 1 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Electronics");
        result.Description.Should().Be("Devices");
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnNull()
    {
        _queryRepository
            .GetByIdAsync(999)
            .Returns((Category?)null);
        var query = new GetCategoryByIdQuery { Id = 999 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        _queryRepository
            .GetByIdAsync(5)
            .Returns(CategoryFixture.CreateCategory(5));
        var query = new GetCategoryByIdQuery { Id = 5 };
        await _handler.Handle(query, CancellationToken.None);
        await _queryRepository
            .Received(1)
            .GetByIdAsync(5);
    }

    [Fact]
    public async Task Handle_WithInactiveCategory_ShouldStillReturnIt()
    {
        var category = CategoryFixture.CreateCategory(1, "Old Category", isActive: false);
        _queryRepository
            .GetByIdAsync(1)
            .Returns(category);
        var query = new GetCategoryByIdQuery { Id = 1 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
    }
}