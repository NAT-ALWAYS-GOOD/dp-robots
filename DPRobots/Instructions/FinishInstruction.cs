namespace DPRobots.Instructions;

public record FinishInstruction(string RobotName) : IInstruction
{
    public override string ToString() => $"FINISHED {RobotName}";
}