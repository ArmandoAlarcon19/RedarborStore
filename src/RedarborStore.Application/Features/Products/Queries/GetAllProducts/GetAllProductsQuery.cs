using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<PaginatedResult<ProductResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}