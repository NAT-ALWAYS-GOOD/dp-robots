using DPRobots.Pieces;

namespace DPRobots.Instructions;

public record AssembleInstruction(string? OutputName, Piece Piece1, Piece Piece2) : IInstruction
{
    public override string ToString()
    {
        return OutputName is null ? $"ASSEMBLE {Piece1} {Piece2}" : $"ASSEMBLE {OutputName} {Piece1} {Piece2}";
    }
}