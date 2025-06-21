using DPRobots.Pieces;

namespace DPRobots.Robots;

public static class RobotBlueprintValidator
{
    private static readonly Dictionary<PieceCategory, HashSet<PieceCategory>> AllowedPieceCategories = new()
    {
        [PieceCategory.Domestic] = [PieceCategory.Domestic, PieceCategory.General, PieceCategory.Industrial],
        [PieceCategory.Industrial] = [PieceCategory.General, PieceCategory.Industrial],
        [PieceCategory.Military] = [PieceCategory.Military, PieceCategory.Industrial],
        [PieceCategory.General] = []
    };

    private static readonly Dictionary<PieceCategory, HashSet<PieceCategory>> AllowedSystemCategories = new()
    {
        [PieceCategory.Domestic] = [PieceCategory.Domestic, PieceCategory.General, PieceCategory.Industrial],
        [PieceCategory.Industrial] = [PieceCategory.General, PieceCategory.Industrial],
        [PieceCategory.Military] = [PieceCategory.Military, PieceCategory.General],
        [PieceCategory.General] = []
    };

    public static bool IsValid(RobotBlueprint blueprint)
    {
        return TryInferCategory(blueprint, out _);
    }

    public static bool TryInferCategory(RobotBlueprint blueprint, out PieceCategory category)
    {
        var pieceCategories = new[]
        {
            blueprint.CorePrototype.Category,
            blueprint.GeneratorPrototype.Category,
            blueprint.GripModulePrototype.Category,
            blueprint.MoveModulePrototype.Category
        };

        var systemCategory = blueprint.SystemPrototype.Category;

        foreach (var cat in Enum.GetValues<PieceCategory>())
        {
            var allowedPieces = AllowedPieceCategories[cat];
            var allowedSystems = AllowedSystemCategories[cat];

            if (pieceCategories.All(c => c.HasValue && allowedPieces.Contains(c.Value))
                && systemCategory.HasValue && allowedSystems.Contains(systemCategory.Value))
            {
                category = cat;
                return true;
            }
        }

        category = default;
        return false;
    }
}