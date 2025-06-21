using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.UserInstructions;

public record AddTemplateUserInstruction : IUserInstruction
{
    public const string CommandName = "ADD_TEMPLATE";

    public override string ToString() => CommandName;

    public static void Execute(string input)
    {
        var parts = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            Logger.Log(LogType.ERROR, "Invalid format. Expected: TEMPLATE_NAME, Piece1, ..., PieceN");
            return;
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
                    Logger.Log(LogType.ERROR, $"Piece `{pieceStr}` is invalid, duplicate, or of unsupported type.");
                    return;
            }
        }

        if (core is null || system is null || generator is null || grip is null || move is null)
        {
            Logger.Log(LogType.ERROR, "Missing one or more required pieces: Core, System, Generator, GripModule, MoveModule.");
            return;
        }

        var blueprint = new RobotBlueprint(name, core, system, generator, grip, move);

        if (!blueprint.IsValid)
        {
            Logger.Log(LogType.ERROR, $"Template `{name}` is invalid and was not added.");
            return;
        }

        RobotTemplates.Add(blueprint);
    }
}
