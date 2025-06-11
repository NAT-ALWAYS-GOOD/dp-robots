using DPRobots.Pieces;

namespace DPRobots.Instructions;

public record GetOutStockInstruction(Piece Piece, int Quantity = 1) : Instruction
{
    public override string ToString() => $"GET_OUT_STOCK {Quantity} {Piece}";
}