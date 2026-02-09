using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;

public class GetAllInventoryMovementsQuery : IRequest<IEnumerable<InventoryMovementResponseDto>>
{
}