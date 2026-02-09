using Domain.Interfaces.Commands;
using MediatR;

namespace Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryCommandRepository _commandRepository;

    public DeleteCategoryCommandHandler(ICategoryCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _commandRepository.DeleteAsync(request.Id);
    }
}