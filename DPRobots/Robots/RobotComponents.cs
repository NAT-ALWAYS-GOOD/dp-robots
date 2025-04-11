using DPRobots.Pieces;

namespace DPRobots.Robots;

public record RobotComponents(
    Core Core,
    Generator Generator,
    GripModule GripModule,
    MoveModule MoveModule
);