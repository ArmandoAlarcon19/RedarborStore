using Application.DTOs.Responses;
using Domain.Interfaces.Queries;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponseDto?>
{
    private readonly IProductQueryRepository _queryRepository;

    public GetProductByIdQueryHandler(IProductQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<ProductResponseDto?> Handle(
        GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _queryRepository.GetByIdAsync(request.Id);
        if (product == null) return null;

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedDate = product.CreatedDate
        };
    }
}