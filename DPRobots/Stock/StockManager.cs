using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager
{
    private List<StockItem> _stock = new();

    private readonly List<StockMovement> _movements = new();

    public void Initialize(List<StockItem> initialStock)
    {
        foreach (var item in initialStock)
        {
            AddStockItem(item, "Initialization");
        }
    }

    public T RemovePiece<T>(Piece piece, string? context = null) where T : Piece
    {
        var stockItem = _stock.Find(item => item.Prototype.Equals(piece));
        if (stockItem == null)
            throw new InvalidOperationException($"No stock item found for piece: {piece}");

        stockItem.DecreaseQuantity(1);
        LogMovement(StockOperation.Remove, piece.ToString(), 1, context);

        return (T)stockItem.Prototype.Clone();
    }

    public void AddStockItem(StockItem item, string? context = null)
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

        LogMovement(StockOperation.Add, item.Prototype.ToString(), 1, context);
    }

    public void AddRobot(Robot? robot, string? context = null)
    {
        if (robot == null) return;

        var robotStockItem = _stock.Find(item => item.Prototype.Equals(robot));
        if (robotStockItem != null)
            robotStockItem.IncreaseQuantity(1);
        else
            _stock.Add(new StockItem(robot, 1));

        LogMovement(StockOperation.Add, robot.ToString(), 1, context);
    }

    public RobotComponents GetRobotComponents(RobotBlueprint blueprint, bool simulate = false, string? context = null)
    {
        switch (simulate)
        {
            case true:
                return new RobotComponents(blueprint.CorePrototype, blueprint.GeneratorPrototype,
                    blueprint.GripModulePrototype, blueprint.MoveModulePrototype);
            case false:
                var core = RemovePiece<Core>(blueprint.CorePrototype, context);
                var generator = RemovePiece<Generator>(blueprint.GeneratorPrototype, context);
                var gripModule = RemovePiece<GripModule>(blueprint.GripModulePrototype, context);
                var moveModule = RemovePiece<MoveModule>(blueprint.MoveModulePrototype, context);

                return new RobotComponents(core, generator, gripModule, moveModule);
        }
    }

    public static Dictionary<Piece, int> CalculateOverallNeededStocks(Dictionary<RobotBlueprint, int> robotRequests,
        bool printDetails = false)
    {
        var overallTotals = new Dictionary<Piece, int>();

        void AddToTotal(Piece piece, int quantity)
        {
            if (!overallTotals.TryAdd(piece, quantity))
                overallTotals[piece] += quantity;
        }

        foreach (var (blueprint, count) in robotRequests)
        {
            List<Piece> neededPieces = blueprint.GetNeededPieces();

            if (printDetails)
            {
                Console.WriteLine($"{count} {blueprint} :");
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

    public bool VerifyRequestedQuantitiesAreAvailable(Dictionary<RobotBlueprint, int> requestedRobotsWithQuantities)
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

    public void LogMovement(StockOperation operation, string itemName, int quantity, string? context = null)
    {
        _movements.Add(new StockMovement(operation, itemName, quantity, context));
    }

    public IEnumerable<StockMovement> GetMovements(string? filter = null)
    {
        return filter == null
            ? _movements
            : _movements.Where(m => m.ItemName.Equals(filter, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<StockItem> GetStock => _stock;
}