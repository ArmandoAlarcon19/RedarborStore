using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetByProductInventoryMovements;

public class GetInventoryMovementsByProductQueryHandler
    : IRequestHandler<GetInventoryMovementsByProductQuery, IEnumerable<InventoryMovementResponseDto>>
{
    private readonly IInventoryMovementQueryRepository _queryRepository;

    public GetInventoryMovementsByProductQueryHandler(IInventoryMovementQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IEnumerable<InventoryMovementResponseDto>> Handle(
        GetInventoryMovementsByProductQuery request, CancellationToken cancellationToken)
    {
        var movements = await _queryRepository.GetByProductAsync(request.ProductId);

        return movements.Select(m => new InventoryMovementResponseDto
        {
            Id = m.Id,
            ProductId = m.ProductId,
            MovementType = m.MovementType,
            Quantity = m.Quantity,
            Reason = m.Reason,
            MovementDate = m.MovementDate
        });
    }
}