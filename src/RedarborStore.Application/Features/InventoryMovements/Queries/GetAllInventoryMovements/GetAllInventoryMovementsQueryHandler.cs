using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;

public class GetAllInventoryMovementsQueryHandler
    : IRequestHandler<GetAllInventoryMovementsQuery, PaginatedResult<InventoryMovementResponseDto>>
{
    private readonly IInventoryMovementQueryRepository _queryRepository;

    public GetAllInventoryMovementsQueryHandler(IInventoryMovementQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<PaginatedResult<InventoryMovementResponseDto>> Handle(
        GetAllInventoryMovementsQuery request, CancellationToken cancellationToken)
    {
        var (movements, totalCount) = await _queryRepository.GetAllAsync(request.PageNumber, request.PageSize);

        var items = movements.Select(m => new InventoryMovementResponseDto
        {
            Id = m.Id,
            ProductId = m.ProductId,
            MovementType = m.MovementType,
            Quantity = m.Quantity,
            Reason = m.Reason,
            MovementDate = m.MovementDate
        });

        return new PaginatedResult<InventoryMovementResponseDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}