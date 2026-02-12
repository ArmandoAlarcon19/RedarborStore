using FluentAssertions;
using NSubstitute;
using RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;
using RedarborStore.Application.Tests.Fixtures;
using RedarborStore.Domain.Enums;
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
    public async Task Handle_WithDefaultPagination_ShouldReturnPaginatedResult()
    {
        var (items, totalCount) = InventoryMovementFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var result = await _handler.Handle(new GetAllInventoryMovementsQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(4);
        result.TotalCount.Should().Be(4);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldMapPropertiesCorrectly()
    {
        var (items, totalCount) = InventoryMovementFixture.CreatePaginatedList();
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((items, totalCount));

        var result = await _handler.Handle(new GetAllInventoryMovementsQuery(), CancellationToken.None);
        var firstMovement = result.Items.First();

        firstMovement.Id.Should().Be(1);
        firstMovement.ProductId.Should().Be(1);
        firstMovement.MovementType.Should().Be(MovementType.Entry);
        firstMovement.Quantity.Should().Be(100);
        firstMovement.Reason.Should().Be("Initial stock");
    }

    [Fact]
    public async Task Handle_WithNoMovements_ShouldReturnEmptyPaginatedResult()
    {
        _queryRepository
            .GetAllAsync(1, 10)
            .Returns((InventoryMovementFixture.CreateEmptyList(), 0));

        var result = await _handler.Handle(new GetAllInventoryMovementsQuery(), CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectPageParameters()
    {
        _queryRepository
            .GetAllAsync(2, 3)
            .Returns((InventoryMovementFixture.CreateEmptyList(), 0));

        var query = new GetAllInventoryMovementsQuery { PageNumber = 2, PageSize = 3 };
        await _handler.Handle(query, CancellationToken.None);

        await _queryRepository
            .Received(1)
            .GetAllAsync(2, 3);
    }

    [Fact]
    public async Task Handle_WithSmallPageSize_ShouldCalculateTotalPagesCorrectly()
    {
        // 4 items con pageSize 2 = 2 páginas
        var movements = InventoryMovementFixture.CreateMovementList().Take(2);
        _queryRepository
            .GetAllAsync(1, 2)
            .Returns((movements, 4));

        var query = new GetAllInventoryMovementsQuery { PageNumber = 1, PageSize = 2 };
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(4);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_DefaultQueryValues_ShouldBePage1Size10()
    {
        var query = new GetAllInventoryMovementsQuery();

        query.PageNumber.Should().Be(1);
        query.PageSize.Should().Be(10);
    }
}