using FluentValidation;
using RedarborStore.Domain.Enums;

namespace RedarborStore.Application.Features.InventoryMovements.Commands.CreateInventoryMovement;

public class CreateInventoryMovementCommandValidator : AbstractValidator<CreateInventoryMovementCommand>
{
    public CreateInventoryMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Debe seleccionar un producto válido.");

        RuleFor(x => x.MovementType)
            .Must(x => x == MovementType.Entry || x == MovementType.Exit)
            .WithMessage("El tipo de movimiento debe ser 'Entry' o 'Exit'.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

        RuleFor(x => x.Reason)
            .MaximumLength(250).WithMessage("La razón no debe exceder 250 caracteres.");
    }
}