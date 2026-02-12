using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Behaviors;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<PaginatedResult<CategoryResponseDto>>, ICacheableQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string CacheKey => $"categories-page-{PageNumber}-size-{PageSize}";
    public TimeSpan? CacheDuration => TimeSpan.FromMinutes(10);
}