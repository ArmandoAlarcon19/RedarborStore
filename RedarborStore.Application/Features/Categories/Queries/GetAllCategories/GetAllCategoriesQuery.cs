using Application.DTOs.Responses;
using MediatR;

namespace Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryResponseDto>>
{
    
}