using Application.DTOs.Responses;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryResponseDto?>
{
    public int Id { get; set; }
}