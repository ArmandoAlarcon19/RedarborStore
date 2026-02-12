using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;

public class GetAllInventoryMovementsQuery : IRequest<PaginatedResult<InventoryMovementResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}