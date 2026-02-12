namespace RedarborStore.Domain.Exceptions;

public class InvalidMovementTypeException : BusinessRuleException
{
    public InvalidMovementTypeException(string movementType)
        : base($"Invalid movement type '{movementType}'. Allowed values: 'Entry', 'Exit'.")
    {
    }
}