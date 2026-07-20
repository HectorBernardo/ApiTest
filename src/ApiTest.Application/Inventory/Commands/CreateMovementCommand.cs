using MediatR;

namespace ApiTest.Application.Inventory.Commands.CreateMovement;

public record CreateMovementCommand(
    int ProductId,
    int Quantity,
    string MovementType, // "Input" or "Output"
    string Reason
) : IRequest<int>;