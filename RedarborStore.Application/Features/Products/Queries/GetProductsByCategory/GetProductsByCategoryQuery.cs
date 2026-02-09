using Application.DTOs.Responses;
using Domain.Interfaces.Queries;
using MediatR;

namespace Application.Features.Products.Queries.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler
    : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductResponseDto>>
{
    private readonly IProductQueryRepository _queryRepository;

    public GetProductsByCategoryQueryHandler(IProductQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IEnumerable<ProductResponseDto>> Handle(
        GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _queryRepository.GetByCategoryAsync(request.CategoryId);

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