using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record GetMovementsUserInstruction(RobotFactory Factory, List<string>? Pieces = null) : IUserInstruction
{
    public const string CommandName = "GET_MOVEMENTS";

    public override string ToString() => GivenArgs != null ? $"{CommandName} {GivenArgs}" : CommandName;

    private static string? GivenArgs { get; set; }

    public static IUserInstruction? TryParse(string args)
    {
        var (piecesArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);
        if (factory is null)
        {
            Logger.Log(LogType.ERROR,
                $"Missing target factory. Available factory for this instruction are {string.Join(", ", FactoryManager.GetInstance().Factories.Select(f => f.Name))}.");
            return null;
        }
        
        if (string.IsNullOrWhiteSpace(args))
            return new GetMovementsUserInstruction(factory);

        try
        {
            var pieceNames = UserInstructionArgumentParser.ParsePieceNames(piecesArgs);
            GivenArgs = args;
            return new GetMovementsUserInstruction(factory, pieceNames);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        var movementsToDisplay = new List<StockMovement>();

        if (Pieces == null || Pieces.Count == 0)
        {
            movementsToDisplay.AddRange(Factory.Stock.GetMovements());
        }
        else
        {
            foreach (var piece in Pieces)
            {
                movementsToDisplay.AddRange(Factory.Stock.GetMovements(piece));
            }
        }

        if (movementsToDisplay.Count == 0)
        {
            Console.WriteLine("Aucun mouvement trouvé.");
            return;
        }

        foreach (var movement in movementsToDisplay.OrderBy(m => m.Timestamp))
        {
            DisplayMovement(movement);
        }
    }


    private static void DisplayMovement(StockMovement movement)
    {
        var colors = new Dictionary<StockOperation, ConsoleColor>
        {
            { StockOperation.Add, ConsoleColor.Green },
            { StockOperation.Remove, ConsoleColor.Red }
        };

        Console.ForegroundColor = colors[movement.Operation];
        Console.WriteLine(movement);
        Console.ResetColor();
    }
}