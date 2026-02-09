using Application.DTOs.Responses;
using MediatR;

namespace RedarborStore.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryResponseDto?>
{
    public int Id { get; set; }
}