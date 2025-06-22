using DPRobots.Instructions;
using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Robots;

namespace DPRobots.UserInstructions;

public record ProduceUserInstruction(Dictionary<RobotBlueprint, int> RobotsWithQuantities, RobotFactory Factory)
    : IUserInstruction
{
    public const string CommandName = "PRODUCE";

    public override string ToString() => $"{CommandName} {GivenArgs}";

    private static string? GivenArgs { get; set; }

    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        var (robotArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);

        try
        {
            var robotsRequest = UserInstructionArgumentParser.ParseRobotsRequest(robotArgs);
            var availableFactories = FactoryManager.GetInstance().GetAvailableFactories(robotsRequest);

            if (factory is null)
            {
                Logger.Log(LogType.ERROR,
                    $"Missing target factory. Available factories for this instruction are {string.Join(", ", availableFactories.Select(f => f.Name))}.");
                return null;
            }

            if (!availableFactories.Contains(factory))
            {
                Logger.Log(LogType.ERROR,
                    $"Factory `{factory.Name}` is not available for the requested robots. Available factories are {string.Join(", ", availableFactories.Select(f => f.Name))}.");
                return null;
            }

            GivenArgs = args;
            var robotsWithQuantities = UserInstructionArgumentParser.ParseRobotsWithQuantities(robotArgs, factory);
            return new ProduceUserInstruction(robotsWithQuantities, factory);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        foreach (var (robotToBuild, count) in RobotsWithQuantities)
        {
            for (var i = 0; i < count; i++)
            {
                var robot = new RobotBuilder(robotToBuild.Name, Factory)
                    .UseTemplate()
                    .Build(true, ToString());
                InstructionsGenerator.GetInstance().GenerateInstructions(robotToBuild);
                Factory.Stock.AddRobot(robot, 1, ToString());
            }
        }

        Logger.Log(LogType.STOCK_UPDATED);
    }
}