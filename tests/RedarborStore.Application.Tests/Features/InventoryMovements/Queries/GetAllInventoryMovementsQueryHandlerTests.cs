using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Tests.Features.InventoryMovements.Queries;

public class GetAllInventoryMovementsQueryHandlerTests
{
    private readonly IInventoryMovementQueryRepository _queryRepository;
    private readonly GetAllInventoryMovementsQueryHandler _handler;

    public GetAllInventoryMovementsQueryHandlerTests()
    {
        _queryRepository = Substitute.For<IInventoryMovementQueryRepository>();
        _handler = new GetAllInventoryMovementsQueryHandler(_queryRepository);
    }

    [Fact]
    public async Task Handle_WithExistingMovements_ShouldReturnAll()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(InventoryMovementFixture.CreateMovementList());
        var result = await _handler.Handle(new GetAllInventoryMovementsQuery(), CancellationToken.None);
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task Handle_ShouldMapPropertiesCorrectly()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(InventoryMovementFixture.CreateMovementList());
        var result = (await _handler.Handle(
            new GetAllInventoryMovementsQuery(), CancellationToken.None)).ToList();
        var firstMovement = result[0];
        firstMovement.Id.Should().Be(1);
        firstMovement.ProductId.Should().Be(1);
        firstMovement.MovementType.Should().Be("Entry");
        firstMovement.Quantity.Should().Be(100);
        firstMovement.Reason.Should().Be("Initial stock");
    }

    [Fact]
    public async Task Handle_WithNoMovements_ShouldReturnEmptyList()
    {
        _queryRepository
            .GetAllAsync()
            .Returns(InventoryMovementFixture.CreateEmptyList());
        var result = await _handler.Handle(new GetAllInventoryMovementsQuery(), CancellationToken.None);
        result.Should().BeEmpty();
    }
}