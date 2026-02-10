using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetInventoryMovementsByProduct;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Enums;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.InventoryMovements.Queries;

public class GetInventoryMovementsByProductQueryHandlerTests
{
    private readonly IInventoryMovementQueryRepository _queryRepository;
    private readonly GetInventoryMovementsByProductQueryHandler _handler;

    public GetInventoryMovementsByProductQueryHandlerTests()
    {
        _queryRepository = Substitute.For<IInventoryMovementQueryRepository>();
        _handler = new GetInventoryMovementsByProductQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingProductMovements_ShouldReturnFiltered()
    {
        _queryRepository
            .GetByProductAsync(1)
            .Returns(InventoryMovementFixture.CreateMovementListByProduct(1));

        var query = new GetInventoryMovementsByProductQuery { ProductId = 1 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(m => m.ProductId.Should().Be(1));
    }

    [Fact]
    public async Task Handle_WithNoMovementsForProduct_ShouldReturnEmptyList()
    {
        _queryRepository
            .GetByProductAsync(999)
            .Returns(InventoryMovementFixture.CreateEmptyList());
        var query = new GetInventoryMovementsByProductQuery { ProductId = 999 };
        var result = await _handler.Handle(query, CancellationToken.None);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectProductId()
    {
        _queryRepository
            .GetByProductAsync(5)
            .Returns(InventoryMovementFixture.CreateEmptyList());
        var query = new GetInventoryMovementsByProductQuery { ProductId = 5 };
        await _handler.Handle(query, CancellationToken.None);
        await _queryRepository.Received(1).GetByProductAsync(5);
    }

    [Fact]
    public async Task Handle_ShouldMapAllDtoFields()
    {
        var movement = InventoryMovementFixture.CreateMovement(
            id: 10,
            productId: 3,
            movementType: MovementType.Exit,
            quantity: 25,
            reason: "Damaged goods");
        _queryRepository
            .GetByProductAsync(3)
            .Returns(new List<Domain.Entities.InventoryMovement> { movement });
        var query = new GetInventoryMovementsByProductQuery { ProductId = 3 };
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(10);
        result[0].ProductId.Should().Be(3);
        result[0].MovementType.Should().Be(MovementType.Exit);
        result[0].Quantity.Should().Be(25);
        result[0].Reason.Should().Be("Damaged goods");
    }
}