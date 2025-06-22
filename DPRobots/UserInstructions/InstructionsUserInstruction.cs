using DPRobots.Instructions;
using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Robots;

namespace DPRobots.UserInstructions;

public record InstructionsUserInstruction(Dictionary<RobotBlueprint, int> RobotsWithQuantities) : IUserInstruction
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
            var robotsRequest = UserInstructionArgumentParser.ParseRobotsRequest(args);
            var factories = FactoryManager.GetInstance().Factories;
            var resolved = new Dictionary<RobotBlueprint, int>();
            foreach (var (robotName, quantity) in robotsRequest)
            {
                var blueprint = factories
                    .Select(f => f.Templates.Get(robotName))
                    .FirstOrDefault(b => b is not null);

                if (blueprint is null)
                {
                    Logger.Log(LogType.ERROR, $"Le robot `{robotName}` n'existe dans aucune usine.");
                    return null;
                }

                if (!resolved.TryAdd(blueprint, quantity))
                    resolved[blueprint] += quantity;
            }
            GivenArgs = args;
            return new InstructionsUserInstruction(resolved);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        foreach (var (blueprint, count) in RobotsWithQuantities)
        {
            for (var i = 0; i < count; i++)
            {
                InstructionsGenerator.GetInstance().PrintInstructions(blueprint);
            }
        }
    }
}