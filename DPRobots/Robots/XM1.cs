using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Xm1 : Robot
{
    protected override string Name => "XM-1";
    
    public override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Cm1, PieceCategory.Military),
        new Generator(GeneratorNames.Gm1, PieceCategory.Military),
        new GripModule(GripModuleNames.Am1, PieceCategory.Military),
        new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military)
    );
}