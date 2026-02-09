using MediatR;

namespace RedarborStore.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}