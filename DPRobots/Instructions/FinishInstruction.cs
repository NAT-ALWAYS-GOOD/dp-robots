namespace DPRobots.Instructions;

public record FinishInstruction(string RobotName) : Instruction
{
    public override string ToString() => $"FINISHED {RobotName}";
}