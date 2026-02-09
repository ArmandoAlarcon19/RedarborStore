using Application.DTOs.Responses;
using Domain.Interfaces.Queries;
using MediatR;

namespace Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;

public class GetAllInventoryMovementsQueryHandler
    : IRequestHandler<GetAllInventoryMovementsQuery, IEnumerable<InventoryMovementResponseDto>>
{
    private readonly IInventoryMovementQueryRepository _queryRepository;

    public GetAllInventoryMovementsQueryHandler(IInventoryMovementQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IEnumerable<InventoryMovementResponseDto>> Handle(
        GetAllInventoryMovementsQuery request, CancellationToken cancellationToken)
    {
        var movements = await _queryRepository.GetAllAsync();

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