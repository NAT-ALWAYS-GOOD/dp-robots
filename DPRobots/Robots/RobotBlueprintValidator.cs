using DPRobots.Pieces;

namespace DPRobots.Robots;

public class RobotBlueprintValidator
{
    public static bool IsValid(RobotBlueprint blueprint)
    {
        RobotCategory category = blueprint.Category;
        
        List<Piece> pieces =
        [
            blueprint.CorePrototype,
            blueprint.GeneratorPrototype,
            blueprint.GripModulePrototype,
            blueprint.MoveModulePrototype
        ];
        List<PieceCategory> allowedPieceCategories = category switch
        {
            RobotCategory.Domestic =>
            [
                PieceCategory.Domestic,
                PieceCategory.General,
                PieceCategory.Industrial
            ],
            RobotCategory.Industrial =>
            [
                PieceCategory.Industrial,
                PieceCategory.General
            ],
            RobotCategory.Military =>
            [
                PieceCategory.Military,
                PieceCategory.Industrial
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
        foreach (Piece piece in pieces)
        {
            PieceCategory? pieceCategory = piece.Category;
            if (pieceCategory == null || !allowedPieceCategories.Contains(pieceCategory.Value))
                return false;
        }
        
        System system = blueprint.SystemPrototype;
        List<PieceCategory> allowedSystemCategories = category switch
        {
            RobotCategory.Domestic => [PieceCategory.Domestic, PieceCategory.General, PieceCategory.Industrial],
            RobotCategory.Industrial => [PieceCategory.General, PieceCategory.Industrial],
            RobotCategory.Military => [PieceCategory.Military, PieceCategory.General],
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
        if (system.Category == null || !allowedSystemCategories.Contains(system.Category.Value))
            return false;

        return true;
    }
}