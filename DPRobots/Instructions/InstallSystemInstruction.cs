namespace DPRobots.Instructions;

using DPRobots.Pieces;

public record InstallSystemInstruction(System System, Core Core) : IInstruction
{
    public override string ToString() => $"INSTALL {System} {Core}";
}