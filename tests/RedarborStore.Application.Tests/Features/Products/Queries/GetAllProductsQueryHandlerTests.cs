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
    public async Task Handle_WithExistingProducts_ShouldReturnAll()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(ProductFixture.CreateProductList());
        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_ShouldMapAllPropertiesCorrectly()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(ProductFixture.CreateProductList());
        var result = (await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None)).ToList();
        var laptop = result.First(p => p.Name == "Laptop");
        laptop.Id.Should().Be(1);
        laptop.Price.Should().Be(1299.99m);
        laptop.Stock.Should().Be(50);
        laptop.CategoryId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithNoProducts_ShouldReturnEmptyList()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(ProductFixture.CreateEmptyList());
        var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}