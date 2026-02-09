namespace RedarborStore.Domain.Entities;

public class InventoryMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; } 
    public string MovementType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Reason { get; set; }
    public DateTime MovementDate { get; set; } = DateTime.Now;
}