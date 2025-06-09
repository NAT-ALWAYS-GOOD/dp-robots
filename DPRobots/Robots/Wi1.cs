using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Wi1 : Robot
{
    protected override string Name => "WI-1";
    
    public override RobotBlueprint Blueprint => new RobotBlueprint(
        "WI-1",
        RobotCategory.Industrial,
        new Core(CoreNames.Ci1, PieceCategory.Industrial),
        new System(SystemNames.Sb1, PieceCategory.General),
        new Generator(GeneratorNames.Gi1, PieceCategory.Industrial),
        new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial),
        new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial)
    );
}