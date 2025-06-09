using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Rd1 : Robot
{
    protected override string Name => "RD-1";
    
    public override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Cd1, PieceCategory.Domestic),
        new Generator(GeneratorNames.Gd1, PieceCategory.Domestic),
        new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic),
        new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic)
    );
}