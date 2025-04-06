using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Xm1 : Robot
{
    protected override string Name => "XM-1";
    
    protected override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Cm1),
        new Generator(GeneratorNames.Gm1),
        new GripModule(GripModuleNames.Am1),
        new MoveModule(MoveModuleNames.Lm1)
    );
}