using Application.DTOs.Responses;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductResponseDto>>
{
}