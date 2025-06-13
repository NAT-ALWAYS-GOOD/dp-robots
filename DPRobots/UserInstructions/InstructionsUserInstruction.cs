using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record InstructionsUserInstruction : IUserInstruction
{
    public const string CommandName = "INSTRUCTIONS";

    public override string ToString() => CommandName;

    public static void Execute(Dictionary<string, int> robotsWithQuantities)
    {
        foreach (var (robotName, count) in robotsWithQuantities)
        {
            for (var i = 0; i < count; i++)
            {
                new RobotBuilder(robotName)
                    .UseTemplate()
                    .GenerateInstructions()
                    .Simulate();
            }
        }
    }
}