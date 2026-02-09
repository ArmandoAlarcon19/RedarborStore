using Domain.Entities;
using Domain.Interfaces.Commands;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryCommandRepository _commandRepository;

    public UpdateCategoryCommandHandler(ICategoryCommandRepository commandRepository)
    {
        _commandRepository = commandRepository;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
        };

        return await _commandRepository.UpdateAsync(category);
    }
}