using Application.DTOs.Responses;
using MediatR;

namespace Application.Features.InventoryMovements.Queries.GetAllInventoryMovements;

public class GetAllInventoryMovementsQuery : IRequest<IEnumerable<InventoryMovementResponseDto>>
{
}