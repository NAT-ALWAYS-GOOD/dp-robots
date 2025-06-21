using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record ProduceUserInstruction : IUserInstruction
{
    public const string CommandName = "PRODUCE";

    public override string ToString() => CommandName;

    public static void Execute(Dictionary<string, int> args)
    {
        var request = args
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
                    .Build();
                StockManager.AddRobot(robot);
            }
        }
        
        Logger.Log(LogType.STOCK_UPDATED);
    }
}