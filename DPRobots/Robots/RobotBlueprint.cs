using DPRobots.Pieces;

namespace DPRobots.Robots;

public record RobotBlueprint(
    string Name,
    RobotCategory Category,
    Core CorePrototype,
    System SystemPrototype,
    Generator GeneratorPrototype,
    GripModule GripModulePrototype,
    MoveModule MoveModulePrototype
);