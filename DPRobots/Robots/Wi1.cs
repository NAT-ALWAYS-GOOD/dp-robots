using DPRobots.Pieces;

namespace DPRobots.Robots;

public class Wi1 : Robot
{
    protected override string Name => "WI-1";
    
    protected override RobotBlueprint Blueprint => new RobotBlueprint(
        new Core(CoreNames.Ci1),
        new Generator(GeneratorNames.Gi1),
        new GripModule(GripModuleNames.Ai1),
        new MoveModule(MoveModuleNames.Li1)
    );
}