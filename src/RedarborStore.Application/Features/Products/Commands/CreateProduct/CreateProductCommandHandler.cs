using MediatR;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductCommandRepository _commandRepository;

    public CreateProductCommandHandler(IProductCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };

        return await _commandRepository.CreateAsync(product);
    }
}