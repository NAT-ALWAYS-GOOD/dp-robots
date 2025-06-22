using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.RobotFactories;
using DPRobots.Robots;

namespace DPRobots.UserInstructions;

public record AddTemplateUserInstruction(RobotBlueprint Blueprint, RobotFactory Factory) : IUserInstruction
{
    public const string CommandName = "ADD_TEMPLATE";

    public override string ToString() => $"{CommandName} {GivenArgs}";

    private static string? GivenArgs { get; set; }
    
    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;
        
        var (blueprintArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);
        if (factory is null)
        {
            Logger.Log(LogType.ERROR,
                $"Missing target factory. Available factory for this instruction are {string.Join(", ", FactoryManager.Factories.Select(f => f.Name))}.");
            return null;
        }
        
        try
        {
            var blueprint = UserInstructionArgumentParser.ParseRobotBlueprint(args);
            GivenArgs = args;
            return new AddTemplateUserInstruction(blueprint, factory);
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
