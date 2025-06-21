using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public class UserInstructionArgumentParser
{
    public static void EnsureEmpty(string args, string commandName)
    {
        if (!string.IsNullOrWhiteSpace(args))
            throw new ArgumentException($"La commande `{commandName}` ne prend pas d'arguments.");
    }
    
    public static Dictionary<string, int> ParseRobotsWithQuantities(string args)
    {
        var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var parts = args.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new ArgumentException(
                    $"Le format de l'argument '{trimmed}' est incorrect. Attendu : 'quantité nom_de_robot'.");

            if (!int.TryParse(tokens[0], out var quantity))
                throw new ArgumentException($"La quantité '{tokens[0]}' n'est pas un nombre valide.");

            var robotName = tokens[1].ToUpper();
            var invalidRobot = RobotTemplates.Get(robotName) == null;
            if (invalidRobot)
                throw new ArgumentException($"Le robot '{robotName}' n'est pas reconnu.");
            if (!result.TryAdd(robotName, quantity))
                result[robotName] += quantity;
        }

        return result;
    }

    public static RobotBlueprint ParseRobotBlueprint(string args)
    {
        var parts = args.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            throw new ArgumentException("Format invalide. Attendu: TEMPLATE_NAME, Piece1, ..., PieceN");
        }

        var name = parts[0].ToUpper();
        var pieceNames = parts[1..];

        Core? core = null;
        System? system = null;
        Generator? generator = null;
        GripModule? grip = null;
        MoveModule? move = null;

        foreach (var pieceStr in pieceNames)
        {
            var piece = PieceFactory.Create(pieceStr);

            switch (piece)
            {
                case Core c when core is null:
                    core = c;
                    break;
                case System s when system is null:
                    system = s;
                    break;
                case Generator g when generator is null:
                    generator = g;
                    break;
                case GripModule gm when grip is null:
                    grip = gm;
                    break;
                case MoveModule mm when move is null:
                    move = mm;
                    break;
                default:
                    throw new ArgumentException($"Pièce invalide ou dupliquée : `{pieceStr}`");
            }
        }

        if (core is null || system is null || generator is null || grip is null || move is null)
        {
            throw new ArgumentException("Il manque une ou plusieurs pièces requises.");
        }

        return new RobotBlueprint(name, core, system, generator, grip, move);
    }
    
    public static List<StockItem> ParseStockItems(string args)
    {
        var result = new List<StockItem>();
        var parts = args.Split(',', StringSplitOptions.RemoveEmptyEntries);
    
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new ArgumentException($"Invalid format for argument '{trimmed}'. Expected: 'quantity name'.");
    
            if (!int.TryParse(tokens[0], out var quantity))
                throw new ArgumentException($"Invalid quantity '{tokens[0]}'. Must be a valid number.");
    
            var name = tokens[1].ToUpper();
            var maybePiece = PieceFactory.TryCreate(name);
            var maybeRobot = Robot.FromName(name);
            if (maybePiece == null && maybeRobot == null)
                throw new ArgumentException($"No piece or robot found with name '{name}'.");
    
            if (maybeRobot != null) result.Add(new StockItem(maybeRobot, quantity));
        }
    
        return result;
    }
}