using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Robot(String name, RobotBlueprint blueprint)
{
    protected string Name { get; } = name;
    public RobotBlueprint Blueprint { get; } = blueprint;
    public Core? Core { get; internal set; }
    public Generator? Generator { get; internal set; }
    public GripModule? GripModule { get; internal set; }
    public MoveModule? MoveModule { get; internal set; }

    /// <summary>
    /// Récupère le robot correspondant au nom donné.
    /// Si le nom ne correspond à aucun robot connu, retourne null.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="printInstructions"></param>
    public static Robot? FromName(string name, bool? printInstructions = true)
    {
        var blueprint = RobotTemplates.Get(name);
        if (blueprint is null) return null;

        var robot = new Robot(name, blueprint: blueprint);
        robot.Core = blueprint.CorePrototype;
        robot.Generator = blueprint.GeneratorPrototype;
        robot.GripModule = blueprint.GripModulePrototype;
        robot.MoveModule = blueprint.MoveModulePrototype;

        return robot;
    }
    
    public List<Piece> GetNeededPieces()
    {
        return
        [
            Blueprint.CorePrototype,
            Blueprint.GeneratorPrototype,
            Blueprint.GripModulePrototype,
            Blueprint.MoveModulePrototype
        ];
    }

    public override string ToString() => Name;
}