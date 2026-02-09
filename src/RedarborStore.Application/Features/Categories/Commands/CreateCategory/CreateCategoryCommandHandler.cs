using MediatR;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Interfaces.Commands;

namespace RedarborStore.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryCommandRepository _commandRepository;

    public CreateCategoryCommandHandler(ICategoryCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        return await _commandRepository.CreateAsync(category);
    }
}