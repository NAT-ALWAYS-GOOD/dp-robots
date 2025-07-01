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
    
    private static readonly Dictionary<PieceCategory, int> MaxAdditionalModules = new()
    {
        [PieceCategory.Domestic] = 1,
        [PieceCategory.Industrial] = 3,
        [PieceCategory.Military] = 2,
        [PieceCategory.General] = 0
    };

    private static bool AreAdditionalModulesValid(RobotBlueprint blueprint)
    {
        var category = blueprint.InferredCategory;
        if (category == null)
        {
            return false;
        }
        
        var additionalModules = blueprint.AdditionalModules ?? [];
        bool allValidTypes = additionalModules.All(module => module is GripModule || module is MoveModule);
        if (!allValidTypes)
            return false;

        var additionalModulesCount = additionalModules.Count;
        var maxAllowed = MaxAdditionalModules[category.Value];
        return additionalModulesCount <= maxAllowed;
    }

    public static bool IsValid(RobotBlueprint blueprint)
    {
        return TryInferCategory(blueprint, out _) && AreAdditionalModulesValid(blueprint);
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