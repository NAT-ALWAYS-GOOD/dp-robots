using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record ProduceUserInstruction(Dictionary<string, int> RobotsWithQuantities) : IUserInstruction
{
    public const string CommandName = "PRODUCE";

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
            return new ProduceUserInstruction(robotsWithQuantities);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }
    
    public void Execute()
    {
        var request = RobotsWithQuantities
            .ToDictionary(kvp => Robot.FromName(kvp.Key)!, kvp => kvp.Value);

        if (!StockManager.VerifyRequestedQuantitiesAreAvailable(request))
        {
            Logger.Log(LogType.ERROR, "Impossible de produire les robots, pas assez de pièces.");
            return;
        }

        foreach (var (robotToBuild, count) in request)
        {
            for (var i = 0; i < count; i++)
            {
                var robot = new RobotBuilder(robotToBuild.ToString())
                    .UseTemplate()
                    .GenerateInstructions()
                    .Build(true, ToString());
                StockManager.AddRobot(robot, ToString());
            }
        }
        
        Logger.Log(LogType.STOCK_UPDATED);
    }
}