using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record InstructionsUserInstruction(Dictionary<string, int> RobotsWithQuantities) : IUserInstruction
{
    public const string CommandName = "INSTRUCTIONS";

    public override string ToString() => $"{CommandName} {GivenArgs}";
    
    private static string? GivenArgs { get; set; }
    
    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        try
        {
            var robotsWithQuantities = UserInstructionArgumentParser.ParseRobotsWithQuantities(args);
            GivenArgs = args;
            return new InstructionsUserInstruction(robotsWithQuantities);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        foreach (var (robotName, count) in RobotsWithQuantities)
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