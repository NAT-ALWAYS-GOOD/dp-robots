using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Rd1 : Robot
{
    protected override string Name => "RD-1";
    
    protected override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Cd1),
        new Generator(GeneratorNames.Gd1),
        new GripModule(GripModuleNames.Ad1),
        new MoveModule(MoveModuleNames.Ld1)
    );
}