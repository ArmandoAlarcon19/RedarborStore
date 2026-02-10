using RedarborStore.Domain.Enums;

namespace RedarborStore.Domain.Entities;

public class InventoryMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; } 
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public string? Reason { get; set; }
    public DateTime MovementDate { get; set; } = DateTime.Now;
}