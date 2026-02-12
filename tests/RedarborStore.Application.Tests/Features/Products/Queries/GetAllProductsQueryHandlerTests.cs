using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Queries.GetAllProducts;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.Products.Queries;

public class GetAllProductsQueryHandlerTests
{
    private readonly IProductQueryRepository _queryRepository;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _queryRepository = Substitute.For<IProductQueryRepository>();
        _handler = new GetAllProductsQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithDefaultPagination_ShouldReturnPaginatedResult()
    {
        var (items, totalCount) = ProductFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldMapAllPropertiesCorrectly()
    {
        var (items, totalCount) = ProductFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);
        var laptop = result.Items.First(p => p.Name == "Laptop");

        laptop.Id.Should().Be(1);
        laptop.Price.Should().Be(1299.99m);
        laptop.Stock.Should().Be(50);
        laptop.CategoryId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithNoProducts_ShouldReturnEmptyPaginatedResult()
    {
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((ProductFixture.CreateEmptyList(), 0));

        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectPageParameters()
    {
        _queryRepository
            .GetAllAsync(3, 5)
            .Returns((ProductFixture.CreateEmptyList(), 0));

        var query = new GetAllProductsQuery { PageNumber = 3, PageSize = 5 };
        await _handler.Handle(query, CancellationToken.None);

        await _queryRepository
            .Received(1)
            .GetAllAsync(3, 5);
    }

    [Fact]
    public async Task Handle_WithMultiplePages_ShouldCalculateTotalPagesCorrectly()
    {
        var (items, totalCount) = ProductFixture.CreateLargePaginatedList(
            pageNumber: 1, pageSize: 10, totalItems: 25);
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var query = new GetAllProductsQuery { PageNumber = 1, PageSize = 10 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalCount.Should().Be(25);
        result.TotalPages.Should().Be(3); // ceil(25/10) = 3
        result.Items.Should().HaveCount(10);
    }

    [Fact]
    public async Task Handle_WithFirstPage_ShouldHaveCorrectNavigation()
    {
        var (items, totalCount) = ProductFixture.CreateLargePaginatedList(
            pageNumber: 1, pageSize: 5, totalItems: 20);
        _queryRepository
            .GetAllAsync(1, 5)
            .Returns((items, totalCount));

        var query = new GetAllProductsQuery { PageNumber = 1, PageSize = 5 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithLastPage_ShouldHaveCorrectNavigation()
    {
        var (items, totalCount) = ProductFixture.CreateLargePaginatedList(
            pageNumber: 4, pageSize: 5, totalItems: 20);
        _queryRepository
            .GetAllAsync(4, 5)
            .Returns((items, totalCount));

        var query = new GetAllProductsQuery { PageNumber = 4, PageSize = 5 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WithSinglePage_ShouldHaveNoNavigation()
    {
        var (items, totalCount) = ProductFixture.CreatePaginatedList(pageNumber: 1, pageSize: 10);
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var query = new GetAllProductsQuery { PageNumber = 1, PageSize = 10 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
        result.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Handle_DefaultQueryValues_ShouldBePage1Size10()
    {
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((ProductFixture.CreateEmptyList(), 0));

        var query = new GetAllProductsQuery(); // sin setear nada

        query.PageNumber.Should().Be(1);
        query.PageSize.Should().Be(10);
    }
}