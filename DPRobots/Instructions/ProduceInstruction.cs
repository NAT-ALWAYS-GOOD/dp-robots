namespace DPRobots.Instructions;

public record ProduceInstruction(string RobotName) : IInstruction
{
    public override string ToString() => $"PRODUCING {RobotName}";
}