using Domain.Entities;
using Domain.Interfaces.Commands;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory;

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