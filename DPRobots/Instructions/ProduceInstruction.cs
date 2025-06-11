namespace DPRobots.Instructions;

public record ProduceInstruction(string RobotName) : Instruction
{
    public override string ToString() => $"PRODUCING {RobotName}";
}