using Application.DTOs.Responses;
using MediatR;
using RedarborStore.Application.Common;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, PaginatedResult<ProductResponseDto>>
{
    private readonly IProductQueryRepository _queryRepository;

    public GetAllProductsQueryHandler(IProductQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<PaginatedResult<ProductResponseDto>> Handle(
        GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _queryRepository.GetAllAsync(request.PageNumber, request.PageSize);

        var items = products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CreatedDate = p.CreatedDate
        });

        return new PaginatedResult<ProductResponseDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}