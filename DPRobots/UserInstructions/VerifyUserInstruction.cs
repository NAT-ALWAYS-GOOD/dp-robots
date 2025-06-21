using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record VerifyUserInstruction(Dictionary<string, int> RobotsWithQuantities) : IUserInstruction
{
    public const string CommandName = "VERIFY";

    public override string ToString() => CommandName;
    
    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        try
        {
            var robotsWithQuantities = UserInstructionArgumentParser.ParseRobotsWithQuantities(args);
            return new VerifyUserInstruction(robotsWithQuantities);
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
        Logger.Log(StockManager.VerifyRequestedQuantitiesAreAvailable(request)
            ? LogType.AVAILABLE
            : LogType.UNAVAILABLE);
    }
}