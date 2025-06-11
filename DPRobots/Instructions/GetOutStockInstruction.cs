using DPRobots.Pieces;

namespace DPRobots.Instructions;

public record GetOutStockInstruction(Piece Piece, int Quantity = 1) : IInstruction
{
    public override string ToString() => $"GET_OUT_STOCK {Quantity} {Piece}";
}