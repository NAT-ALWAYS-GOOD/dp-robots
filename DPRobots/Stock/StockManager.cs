using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager
{
    private List<StockItem> _stock = new();
    
    private readonly List<RobotStockItem> _robotStocks = new();

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

    public T RemovePiece<T>(Piece piece) where T : Piece
    {
        var stockItem = _stock.Find(item => item.Prototype.Equals(piece));
        if (stockItem == null)
            throw new InvalidOperationException($"No stock item found for piece: {piece}");

        stockItem.DecreaseQuantity(1);

        return (T)stockItem.Prototype.Clone();
    }

    public void AddRobot(Robot robot)
    {
        var robotStockItem = _robotStocks.Find(item => item.RobotPrototype == robot);
        if (robotStockItem != null)
            robotStockItem.IncreaseQuantity(1);
        else
            _robotStocks.Add(new RobotStockItem(robot, 1));
    }
    
    public RobotComponents GetRobotComponents(RobotBlueprint blueprint, bool simulate = false)
    {
        var core = simulate ? blueprint.CorePrototype : RemovePiece<Core>(blueprint.CorePrototype);
        var generator = simulate ? blueprint.GeneratorPrototype : RemovePiece<Generator>(blueprint.GeneratorPrototype);
        var gripModule = simulate ? blueprint.GripModulePrototype : RemovePiece<GripModule>(blueprint.GripModulePrototype);
        var moveModule = simulate ? blueprint.MoveModulePrototype : RemovePiece<MoveModule>(blueprint.MoveModulePrototype);

        return new RobotComponents(core, generator, gripModule, moveModule);
    }
    //
    // public Dictionary<string, int> Summarize(Dictionary<Robot, int> robotsRequest)
    // {
    //     
    // }


    private static Dictionary<string, int> CalculateOverallNeededStocks(Dictionary<string, int> robotRequests,
        bool printDetails = false)
    {
        var overallTotals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        void AddToTotal(string pieceName, int quantity)
        {
            if (!overallTotals.TryAdd(pieceName, quantity))
                overallTotals[pieceName] += quantity;
        }

        foreach (var (robotName, count) in robotRequests)
        {
            var robot = Robot.FromName(robotName);
            if (robot == null) continue;

            var pieceNames = GetNeededPieceNames(robot);

            if (printDetails)
            {
                Console.WriteLine($"{count} {robotName} :");
                foreach (var pieceName in pieceNames)
                {
                    Console.WriteLine($"    {count} {pieceName}");
                }
            }

            foreach (var pieceName in pieceNames)
            {
                AddToTotal(pieceName, count);
            }
        }

        return overallTotals;

        IEnumerable<string> GetNeededPieceNames(Robot robot)
        {
            var blueprint = robot.Blueprint;
            return
            [
                blueprint.CorePrototype.ToString(),
                blueprint.GeneratorPrototype.ToString(),
                blueprint.GripModulePrototype.ToString(),
                blueprint.MoveModulePrototype.ToString()
            ];
        }
    }

    public IReadOnlyList<StockItem> GetPieceStock => _stock;
    public IReadOnlyList<RobotStockItem> GetRobotStocks => _robotStocks;
}