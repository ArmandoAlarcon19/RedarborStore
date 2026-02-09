using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>
{
    private readonly IProductQueryRepository _queryRepository;

    public GetAllProductsQueryHandler(IProductQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IEnumerable<ProductResponseDto>> Handle(
        GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _queryRepository.GetAllAsync();

        return products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CreatedDate = p.CreatedDate
        });
    }
}