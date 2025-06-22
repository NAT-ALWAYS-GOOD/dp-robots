using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record VerifyUserInstruction(Dictionary<RobotBlueprint, int> RobotsWithQuantities, RobotFactory Factory)
    : IUserInstruction
{
    public const string CommandName = "VERIFY";

    public override string ToString() => $"{CommandName} {GivenArgs}";

    private static string? GivenArgs { get; set; }

    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        var (robotArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);
        if (factory is null)
        {
            Logger.Log(LogType.ERROR,
                $"Missing target factory. Available factory for this instruction are {string.Join(", ", FactoryManager.Factories.Select(f => f.Name))}.");
            return null;
        }

        try
        {
            var robotsWithQuantities = UserInstructionArgumentParser.ParseRobotsWithQuantities(robotArgs);
            GivenArgs = args;
            return new VerifyUserInstruction(robotsWithQuantities, factory);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        Logger.Log(Factory.Stock.VerifyRequestedQuantitiesAreAvailable(RobotsWithQuantities)
            ? LogType.AVAILABLE
            : LogType.UNAVAILABLE);
    }
}