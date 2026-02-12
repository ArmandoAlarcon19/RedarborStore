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
    public async Task Handle_WithDefaultPagination_ShouldReturnPaginatedResult()
    {
        var (items, totalCount) = CategoryFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldMapPropertiesCorrectly()
    {
        var (items, totalCount) = CategoryFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);
        var list = result.Items.ToList();

        list[0].Id.Should().Be(1);
        list[0].Name.Should().Be("Electronics");
        list[0].Description.Should().Be("Electronic devices");
        list[1].Id.Should().Be(2);
        list[1].Name.Should().Be("Clothing");
        list[2].Id.Should().Be(3);
        list[2].Name.Should().Be("Food & Beverages");
    }

    [Fact]
    public async Task Handle_WithNoCategories_ShouldReturnEmptyPaginatedResult()
    {
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((CategoryFixture.CreateEmptyList(), 0));

        var query = new GetAllCategoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectPageParameters()
    {
        _queryRepository
            .GetAllAsync(2, 5)
            .Returns((CategoryFixture.CreateEmptyList(), 0));

        var query = new GetAllCategoriesQuery { PageNumber = 2, PageSize = 5 };
        await _handler.Handle(query, CancellationToken.None);

        await _queryRepository
            .Received(1)
            .GetAllAsync(2, 5);
    }

    [Fact]
    public async Task Handle_WithCustomPageSize_ShouldReturnCorrectPageSize()
    {
        var (items, totalCount) = CategoryFixture.CreateLargePaginatedList(
            pageNumber: 1, pageSize: 2, totalItems: 10);
        _queryRepository
            .GetAllAsync(1, 2)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery { PageNumber = 1, PageSize = 2 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageSize.Should().Be(2);
        result.TotalCount.Should().Be(10);
        result.TotalPages.Should().Be(5);
    }

    [Fact]
    public async Task Handle_WithFirstPage_ShouldNotHavePreviousPage()
    {
        var (items, totalCount) = CategoryFixture.CreateLargePaginatedList(
            pageNumber: 1, pageSize: 5, totalItems: 15);
        _queryRepository
            .GetAllAsync(1, 5)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery { PageNumber = 1, PageSize = 5 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithMiddlePage_ShouldHaveBothNavigation()
    {
        var (items, totalCount) = CategoryFixture.CreateLargePaginatedList(
            pageNumber: 2, pageSize: 5, totalItems: 15);
        _queryRepository
            .GetAllAsync(2, 5)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery { PageNumber = 2, PageSize = 5 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
        result.PageNumber.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithLastPage_ShouldNotHaveNextPage()
    {
        var (items, totalCount) = CategoryFixture.CreateLargePaginatedList(
            pageNumber: 3, pageSize: 5, totalItems: 15);
        _queryRepository
            .GetAllAsync(3, 5)
            .Returns((items, totalCount));

        var query = new GetAllCategoriesQuery { PageNumber = 3, PageSize = 5 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TotalPages_ShouldCalculateCorrectly()
    {
        _queryRepository
            .GetAllAsync(1, 3)
            .Returns((CategoryFixture.CreateCategoryList().Take(3), 7));

        var query = new GetAllCategoriesQuery { PageNumber = 1, PageSize = 3 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalPages.Should().Be(3); // ceil(7/3) = 3
    }
}