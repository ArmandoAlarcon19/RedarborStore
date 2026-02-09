using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, CategoryResponseDto?>
{
    private readonly ICategoryQueryRepository _queryRepository;

    public GetCategoryByIdQueryHandler(ICategoryQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<CategoryResponseDto?> Handle(
        GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _queryRepository.GetByIdAsync(request.Id);
        if (category == null) return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedDate = category.CreatedDate
        };
    }
}