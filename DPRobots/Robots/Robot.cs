using DPRobots.Pieces;
using DPRobots.RobotFactories;

namespace DPRobots.Robots;

public class Robot : Piece
{
    protected string Name { get; }
    public RobotBlueprint Blueprint { get; }
    public Core? Core { get; internal set; }
    public Generator? Generator { get; internal set; }
    public GripModule? GripModule { get; internal set; }
    public MoveModule? MoveModule { get; internal set; }
    public List<Piece>? AdditionalModules { get; internal set; }
    
    public Robot(string name, RobotBlueprint? blueprint)
        : base(name, blueprint?.InferredCategory)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Robot name cannot be null or empty.", nameof(name));

        if (blueprint is null)
            throw new ArgumentNullException(nameof(blueprint), "Blueprint cannot be null.");

        if (!blueprint.IsValid)
            throw new ArgumentException($"Invalid blueprint for robot '{name}'. The inferred category cannot be 'General'.");

        Name = name;
        Blueprint = blueprint;

        Core = blueprint.CorePrototype;
        Generator = blueprint.GeneratorPrototype;
        GripModule = blueprint.GripModulePrototype;
        MoveModule = blueprint.MoveModulePrototype;
        AdditionalModules = blueprint.AdditionalModules;
    }

    /// <summary>
    /// Récupère le robot correspondant au nom donné.
    /// Si le nom ne correspond à aucun robot connu, retourne null.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="factoryFound"></param>
    /// 
    public static Robot? FromName(string name, out RobotFactory? factoryFound)
    {
        foreach (var factory in FactoryManager.GetInstance().Factories)
        {
            var blueprint = factory.Templates.Get(name);
            if (blueprint is not null)
            {
                factoryFound = factory;
                var robot = new Robot(name, blueprint);
                robot.Core = blueprint.CorePrototype;
                robot.Generator = blueprint.GeneratorPrototype;
                robot.GripModule = blueprint.GripModulePrototype;
                robot.MoveModule = blueprint.MoveModulePrototype;
                robot.AdditionalModules = blueprint.AdditionalModules;
                return robot;
            }
        }

        factoryFound = null;
        return null;
    }


    public override object Clone()
    {
        var clonedRobot = new Robot(Name, Blueprint)
        {
            Core = Core?.Clone() as Core,
            Generator = Generator?.Clone() as Generator,
            GripModule = GripModule?.Clone() as GripModule,
            MoveModule = MoveModule?.Clone() as MoveModule,
            AdditionalModules = AdditionalModules?
                .Select(m => m.Clone() as Piece)
                .OfType<Piece>()
                .ToList()
        };
        return clonedRobot;
    }

    public override string ToString() => Name;
}