using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetByProductInventoryMovements;

public class GetInventoryMovementsByProductQuery : IRequest<IEnumerable<InventoryMovementResponseDto>>
{
    public int ProductId { get; set; }
}