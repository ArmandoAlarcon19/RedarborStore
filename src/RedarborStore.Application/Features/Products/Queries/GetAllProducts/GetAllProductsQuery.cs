using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Behaviors;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<PaginatedResult<ProductResponseDto>>, ICacheableQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string CacheKey => $"products-page-{PageNumber}-size-{PageSize}";
    public TimeSpan? CacheDuration => TimeSpan.FromMinutes(5);
}