using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponseDto>>
{
    private readonly ICategoryQueryRepository _queryRepository;

    public GetAllCategoriesQueryHandler(ICategoryQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> Handle(
        GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _queryRepository.GetAllAsync();

        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedDate = c.CreatedDate
        });
    }
}