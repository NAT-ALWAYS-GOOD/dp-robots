using DPRobots.Logging;
using DPRobots.Pieces;

namespace DPRobots.Robots;

public class RobotTemplates
{
    private readonly Dictionary<string, RobotBlueprint> _templates = new();

    public void Add(RobotBlueprint template)
    {
        if (!RobotBlueprintValidator.IsValid(template))
            Logger.Log(LogType.ERROR, $"Le template {template.Name} n'est pas valide");
        _templates[template.Name] = template;
    }

    public RobotBlueprint? Get(string name)
    {
        return _templates.GetValueOrDefault(name);
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