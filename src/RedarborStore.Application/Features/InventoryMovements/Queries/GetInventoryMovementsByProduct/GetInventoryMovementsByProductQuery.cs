using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetInventoryMovementsByProduct;

public class GetInventoryMovementsByProductQuery : IRequest<IEnumerable<InventoryMovementResponseDto>>
{
    public int ProductId { get; set; }
}