using MediatR;
using RedarborStore.Domain.Entities;
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
        // Validate product exists (Query → EF Core)
        var product = await _productQueryRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new Exception($"Product with ID {request.ProductId} not found");

        // Validate movement type
        if (request.MovementType != "Entry" && request.MovementType != "Exit")
            throw new Exception("Movement type must be 'Entry' or 'Exit'");

        // Validate sufficient stock for exits
        if (request.MovementType == "Exit" && product.Stock < request.Quantity)
            throw new Exception($"Insufficient stock. Current stock: {product.Stock}");

        // Calculate new stock
        var newStock = request.MovementType == "Entry"
            ? product.Stock + request.Quantity
            : product.Stock - request.Quantity;

        // Update product stock (Command → Dapper)
        await _productCommandRepository.UpdateStockAsync(product.Id, newStock);

        // Create movement record (Command → Dapper)
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