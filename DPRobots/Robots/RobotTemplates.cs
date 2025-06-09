using DPRobots.Logging;
using DPRobots.Pieces;

namespace DPRobots.Robots;

public class RobotTemplates
{
    private static readonly Dictionary<string, RobotBlueprint> Templates = new();

    private static void Add(RobotBlueprint template)
    {
        if (!RobotBlueprintValidator.IsValid(template))
            Logger.Log(LogType.ERROR, $"Le template {template.Name} n'est pas valide");
        Templates[template.Name] = template;
    }

    public static RobotBlueprint? Get(string name)
    {
        return Templates.GetValueOrDefault(name);
    }

    public static void InitializeTemplates()
    {
        Add(new RobotBlueprint(
            "XM-1",
            RobotCategory.Military,
            new Core(CoreNames.Cm1, PieceCategory.Military),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gm1, PieceCategory.Military),
            new GripModule(GripModuleNames.Am1, PieceCategory.Military),
            new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military)
        ));

        Add(new RobotBlueprint(
            "RD-1",
            RobotCategory.Domestic,
            new Core(CoreNames.Cd1, PieceCategory.Domestic),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gd1, PieceCategory.Domestic),
            new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic),
            new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic)
        ));

        Add(new RobotBlueprint(
            "WI-1",
            RobotCategory.Industrial,
            new Core(CoreNames.Ci1, PieceCategory.Industrial),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gi1, PieceCategory.Industrial),
            new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial),
            new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial)
        ));
    }
}