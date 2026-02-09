using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Categories.Queries.GetAllCategories;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.Categories.Queries;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly ICategoryQueryRepository _queryRepository;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _queryRepository = Substitute.For<ICategoryQueryRepository>();
        _handler = new GetAllCategoriesQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingCategories_ShouldReturnAll()
    {
        var categories = CategoryFixture.CreateCategoryList();
        _queryRepository
            .GetAllAsync()
            .Returns(categories);
        var query = new GetAllCategoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_WithExistingCategories_ShouldMapCorrectly()
    {
        var categories = CategoryFixture.CreateCategoryList();
        _queryRepository
            .GetAllAsync()
            .Returns(categories);
        var query = new GetAllCategoriesQuery();
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Electronics");
        result[0].Description.Should().Be("Electronic devices");
        result[1].Id.Should().Be(2);
        result[1].Name.Should().Be("Clothing");
        result[2].Id.Should().Be(3);
        result[2].Name.Should().Be("Food & Beverages");
    }

    [Fact]
    public async Task Handle_WithNoCategories_ShouldReturnEmptyList()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(CategoryFixture.CreateEmptyList());
        var query = new GetAllCategoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryExactlyOnce()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(CategoryFixture.CreateCategoryList());
        var query = new GetAllCategoriesQuery();
        await _handler.Handle(query, CancellationToken.None);
        await _queryRepository
            .Received(1)
            .GetAllAsync();
    }
}