using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager
{
    private static List<StockItem> _stock = new();

    private static StockManager? _instance;

    public static StockManager GetInstance()
    {
        if (_instance != null)
            return _instance;

        return _instance = new StockManager();
    }

    public void Initialize(List<StockItem> initialStock)
    {
        _stock = initialStock;
        _instance = this;
    }

    public static T RemovePiece<T>(Piece piece) where T : Piece
    {
        var stockItem = _stock.Find(item => item.Prototype.Equals(piece));
        if (stockItem == null)
            throw new InvalidOperationException($"No stock item found for piece: {piece}");

        stockItem.DecreaseQuantity(1);

        return (T)stockItem.Prototype.Clone();
    }
    
    public void AddStockItem(StockItem item)
    {
        var existingItem = _stock.Find(stock => stock.Prototype.Equals(item.Prototype));
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(item.Quantity);
        }
        else
        {
            _stock.Add(item);
        }
    }

    public static void AddRobot(Robot? robot)
    {
        if (robot == null) return;
        
        var robotStockItem = _stock.Find(item => item.Prototype.Equals(robot));
        if (robotStockItem != null)
            robotStockItem.IncreaseQuantity(1);
        else
            _stock.Add(new StockItem(robot, 1));
    }

    public static RobotComponents GetRobotComponents(RobotBlueprint blueprint, bool simulate = false)
    {
        var core = simulate ? blueprint.CorePrototype : RemovePiece<Core>(blueprint.CorePrototype);
        var generator = simulate ? blueprint.GeneratorPrototype : RemovePiece<Generator>(blueprint.GeneratorPrototype);
        var gripModule =
            simulate ? blueprint.GripModulePrototype : RemovePiece<GripModule>(blueprint.GripModulePrototype);
        var moveModule =
            simulate ? blueprint.MoveModulePrototype : RemovePiece<MoveModule>(blueprint.MoveModulePrototype);

        return new RobotComponents(core, generator, gripModule, moveModule);
    }
    //
    // public Dictionary<string, int> Summarize(Dictionary<Robot, int> robotsRequest)
    // {
    //     
    // }


    public static Dictionary<Piece, int> CalculateOverallNeededStocks(Dictionary<Robot, int> robotRequests,
        bool printDetails = false)
    {
        var overallTotals = new Dictionary<Piece, int>();

        void AddToTotal(Piece piece, int quantity)
        {
            if (!overallTotals.TryAdd(piece, quantity))
                overallTotals[piece] += quantity;
        }

        foreach (var (robot, count) in robotRequests)
        {
            List<Piece> neededPieces = robot.GetNeededPieces();

            if (printDetails)
            {
                Console.WriteLine($"{count} {robot} :");
                foreach (var piece in neededPieces)
                {
                    Console.WriteLine($"    {count} {piece}");
                }
            }

            foreach (var piece in neededPieces)
            {
                AddToTotal(piece, count);
            }
        }

        return overallTotals;
    }

    public static bool VerifyRequestedQuantitiesAreAvailable(Dictionary<Robot, int> requestedRobotsWithQuantities)
    {
        var overallTotals = CalculateOverallNeededStocks(requestedRobotsWithQuantities);

        foreach (var pieceWithQuantity in overallTotals)
        {
            var available = _stock.Where(stockItem => stockItem.Prototype.Equals(pieceWithQuantity.Key))
                .Sum(stockItem => stockItem.Quantity);
            if (available >= pieceWithQuantity.Value)
                continue;

            return false;
        }

        return true;
    }

    public IReadOnlyList<StockItem> GetStock => _stock;
}