using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.Products.Queries.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductResponseDto>>
{
    public int CategoryId { get; set; }
}