using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record InstructionsUserInstruction : IUserInstruction
{
    public override string ToString() => "INSTRUCTIONS";

    public static void Execute(Dictionary<string, int> robotsWithQuantities)
    {
        foreach (var (robotName, count) in robotsWithQuantities)
        {
            var robotToBuild = Robot.FromName(robotName);
            if (robotToBuild == null) continue;

            for (var i = 0; i < count; i++)
            {
                var robotComponents = StockManager.GetRobotComponents(robotToBuild.Blueprint, true);
                robotToBuild.Build(robotComponents, robotToBuild.Blueprint.SystemPrototype, true);
            }
        }
    }
}