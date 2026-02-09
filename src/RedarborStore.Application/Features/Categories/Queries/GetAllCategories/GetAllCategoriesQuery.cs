using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryResponseDto>>
{
    
}