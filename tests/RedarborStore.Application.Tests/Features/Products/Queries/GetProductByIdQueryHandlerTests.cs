using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.Products.Queries.GetProductById;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly IProductQueryRepository _queryRepository;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _queryRepository = Substitute.For<IProductQueryRepository>();
        _handler = new GetProductByIdQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingId_ShouldReturnProduct()
    {
        _queryRepository
            .GetByIdAsync(1)
            .Returns(ProductFixture.CreateProduct());
        var query = new GetProductByIdQuery { Id = 1 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(1299.99m);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnNull()
    {
        _queryRepository
            .GetByIdAsync(999)
            .Returns((Product?)null);
        var query = new GetProductByIdQuery { Id = 999 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeNull();
    }
}