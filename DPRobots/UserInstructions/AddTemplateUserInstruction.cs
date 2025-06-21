using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.UserInstructions;

public record AddTemplateUserInstruction(RobotBlueprint Blueprint) : IUserInstruction
{
    public const string CommandName = "ADD_TEMPLATE";

    public override string ToString() => CommandName;
    
    public static IUserInstruction? TryParse(string args)
    {
        try
        {
            var blueprint = UserInstructionArgumentParser.ParseRobotBlueprint(args);
            return new AddTemplateUserInstruction(blueprint);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {

        if (!Blueprint.IsValid)
        {
            Logger.Log(LogType.ERROR, $"Template `{Blueprint.Name}` is invalid and was not added.");
            return;
        }

        RobotTemplates.Add(Blueprint);
    }
}
