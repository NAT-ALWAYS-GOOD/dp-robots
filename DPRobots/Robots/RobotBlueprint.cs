using DPRobots.Pieces;

namespace DPRobots.Robots;

public record RobotBlueprint(
    Core CorePrototype,
    Generator GeneratorPrototype,
    GripModule GripModulePrototype,
    MoveModule MoveModulePrototype
);