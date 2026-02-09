using Application.DTOs.Responses;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductResponseDto?>
{
    public int Id { get; set; }
}