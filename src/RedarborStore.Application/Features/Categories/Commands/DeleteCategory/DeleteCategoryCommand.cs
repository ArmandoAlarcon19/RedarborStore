using MediatR;

namespace RedarborStore.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
}