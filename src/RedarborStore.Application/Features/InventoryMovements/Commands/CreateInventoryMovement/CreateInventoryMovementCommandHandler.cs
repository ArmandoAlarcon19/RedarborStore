using MediatR;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Enums;
using RedarborStore.Domain.Exceptions;
using RedarborStore.Domain.Interfaces.Commands;
using RedarborStore.Domain.Interfaces.Queries;

namespace RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;

public class CreateInventoryMovementCommandHandler
    : IRequestHandler<CreateInventoryMovementCommand, int>
{
    private readonly IInventoryMovementCommandRepository _movementCommandRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IProductCommandRepository _productCommandRepository;

    public CreateInventoryMovementCommandHandler(
        IInventoryMovementCommandRepository movementCommandRepository,
        IProductQueryRepository productQueryRepository,
        IProductCommandRepository productCommandRepository)
    {
        _movementCommandRepository = movementCommandRepository;
        _productQueryRepository = productQueryRepository;
        _productCommandRepository = productCommandRepository;
    }

    public async Task<int> Handle(
        CreateInventoryMovementCommand request, CancellationToken cancellationToken)
    {
        var product = await _productQueryRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new NotFoundException(nameof(product),request.ProductId);

        if (request.MovementType != MovementType.Entry && request.MovementType != MovementType.Exit)
            throw new InvalidMovementTypeException("Movement type must be 'Entry' or 'Exit'");

        if (request.MovementType == MovementType.Exit && product.Stock < request.Quantity)
            throw new InsufficientStockException(product.Id,product.Stock, request.Quantity);

        var newStock = request.MovementType == MovementType.Entry
            ? product.Stock + request.Quantity
            : product.Stock - request.Quantity;

        await _productCommandRepository.UpdateStockAsync(product.Id, newStock);

        var movement = new InventoryMovement
        {
            ProductId = request.ProductId,
            MovementType = request.MovementType,
            Quantity = request.Quantity,
            Reason = request.Reason
        };

        return await _movementCommandRepository.CreateAsync(movement);
    }
}