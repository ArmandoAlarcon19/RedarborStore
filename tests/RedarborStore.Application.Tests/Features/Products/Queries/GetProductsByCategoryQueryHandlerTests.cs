using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Queries.GetProductsByCategory;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.Products.Queries;

public class GetProductsByCategoryQueryHandlerTests
{
    private readonly IProductQueryRepository _queryRepository;
    private readonly GetProductsByCategoryQueryHandler _handler;

    public GetProductsByCategoryQueryHandlerTests()
    {
        _queryRepository = Substitute.For<IProductQueryRepository>();
        _handler = new GetProductsByCategoryQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingCategory_ShouldReturnProducts()
    {
        _queryRepository
            .GetByCategoryAsync(1)
            .Returns(ProductFixture.CreateProductListByCategory(1));
        var query = new GetProductsByCategoryQuery { CategoryId = 1 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(p => p.CategoryId.Should().Be(1));
    }

    [Fact]
    public async Task Handle_WithNonExistentCategory_ShouldReturnEmptyList()
    {
        _queryRepository
            .GetByCategoryAsync(999)
            .Returns(ProductFixture.CreateEmptyList());
        var query = new GetProductsByCategoryQuery { CategoryId = 999 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectCategoryId()
    {
        _queryRepository
            .GetByCategoryAsync(2)
            .Returns(ProductFixture.CreateProductListByCategory(2));
        var query = new GetProductsByCategoryQuery { CategoryId = 2 };
        await _handler.Handle(query, CancellationToken.None);
        await _queryRepository.Received(1).GetByCategoryAsync(2);
    }
}