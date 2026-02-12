using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, PaginatedResult<CategoryResponseDto>>
{
    private readonly ICategoryQueryRepository _queryRepository;

    public GetAllCategoriesQueryHandler(ICategoryQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<PaginatedResult<CategoryResponseDto>> Handle(
        GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var (categories, totalCount) = await _queryRepository.GetAllAsync(request.PageNumber, request.PageSize);

        var items = categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedDate = c.CreatedDate
        });

        return new PaginatedResult<CategoryResponseDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}