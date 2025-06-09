using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Wi1 : Robot
{
    protected override string Name => "WI-1";
    
    public override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Ci1, PieceCategory.Industrial),
        new Generator(GeneratorNames.Gi1, PieceCategory.Industrial),
        new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial),
        new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial)
    );
}