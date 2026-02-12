namespace RedarborStore.Domain.Exceptions;

public class InsufficientStockException : BusinessRuleException
{
    public InsufficientStockException(int productId, int currentStock, int requestedQuantity)
        : base($"Insufficient stock for product {productId}. Current: {currentStock}, Requested: {requestedQuantity}")
    {
        ProductId = productId;
        CurrentStock = currentStock;
        RequestedQuantity = requestedQuantity;
    }

    public int ProductId { get; }
    public int CurrentStock { get; }
    public int RequestedQuantity { get; }
}