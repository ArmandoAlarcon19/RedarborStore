using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<PaginatedResult<CategoryResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}