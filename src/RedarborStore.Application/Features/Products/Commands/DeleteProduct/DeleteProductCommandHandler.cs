using MediatR;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductCommandRepository _commandRepository;

    public DeleteProductCommandHandler(IProductCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await _commandRepository.DeleteAsync(request.Id);
    }
}