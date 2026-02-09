using Domain.Entities;
using Domain.Interfaces.Commands;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductCommandRepository _commandRepository;

    public UpdateProductCommandHandler(IProductCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            UpdatedDate = DateTime.Now
        };

        return await _commandRepository.UpdateAsync(product);
    }
}