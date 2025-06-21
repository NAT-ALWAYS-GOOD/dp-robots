using DPRobots.Pieces;

namespace DPRobots.Robots;

public record RobotBlueprint(
    string Name,
    Core CorePrototype,
    System SystemPrototype,
    Generator GeneratorPrototype,
    GripModule GripModulePrototype,
    MoveModule MoveModulePrototype
)
{
    public RobotCategory? InferredCategory =>
        RobotBlueprintValidator.TryInferCategory(this, out var category)
            ? category
            : null;

    public bool IsValid => InferredCategory != null;
}