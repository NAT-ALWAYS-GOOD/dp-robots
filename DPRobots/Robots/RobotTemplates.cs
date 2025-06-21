using DPRobots.Logging;
using DPRobots.Pieces;

namespace DPRobots.Robots;

public class RobotTemplates
{
    private static readonly Dictionary<string, RobotBlueprint> Templates = new();
    
    private static RobotTemplates? _instance;

    public static RobotTemplates GetInstance()
    {
        if (_instance != null)
            return _instance;

        return _instance = new RobotTemplates();
    }

    public static void Add(RobotBlueprint template)
    {
        if (!RobotBlueprintValidator.IsValid(template))
            Logger.Log(LogType.ERROR, $"Le template {template.Name} n'est pas valide");
        Templates[template.Name] = template;
    }

    public static RobotBlueprint? Get(string name)
    {
        return Templates.GetValueOrDefault(name);
    }

    public void InitializeTemplates()
    {
        Add(new RobotBlueprint(
            "XM-1",
            (Core)PieceFactory.Create("Core_CM1"),
            (System)PieceFactory.Create("System_SB1"),
            (Generator)PieceFactory.Create("Generator_GM1"),
            (GripModule)PieceFactory.Create("Arms_AM1"),
            (MoveModule)PieceFactory.Create("Legs_LM1")
        ));

        Add(new RobotBlueprint(
            "RD-1",
            (Core)PieceFactory.Create("Core_CD1"),
            (System)PieceFactory.Create("System_SB1"),
            (Generator)PieceFactory.Create("Generator_GD1"),
            (GripModule)PieceFactory.Create("Arms_AD1"),
            (MoveModule)PieceFactory.Create("Legs_LD1")
        ));

        Add(new RobotBlueprint(
            "WI-1",
            (Core)PieceFactory.Create("Core_CI1"),
            (System)PieceFactory.Create("System_SB1"),
            (Generator)PieceFactory.Create("Generator_GI1"),
            (GripModule)PieceFactory.Create("Arms_AI1"),
            (MoveModule)PieceFactory.Create("Legs_LI1")
        ));
    }
}