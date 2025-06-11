using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager
{
    private Dictionary<string, StockItem> _stock = new();
    
    private readonly Dictionary<string, RobotStockItem> _robotStocks = new();

    private static StockManager? _instance;

    public static StockManager GetInstance()
    {
        if (_instance != null)
            return _instance;

        return _instance = new StockManager();
    }
    
    public void Initialize(Dictionary<string, StockItem> initialStock)
    {
        _stock = initialStock;
        _instance = this;
    }

    public T RemovePiece<T>(string key) where T : Piece
    {
        var stockItem = _stock.GetValueOrDefault(key);
        if (stockItem == null)
            throw new InvalidOperationException($"No stock item found for key: {key}");

        stockItem.DecreaseQuantity(1);

        return (T)stockItem.Prototype.Clone();
    }

    public void AddRobot(Robot robot)
    {
        var key = robot.ToString();
        if (_robotStocks.TryGetValue(key, out var robotStockItem))
            robotStockItem.IncreaseQuantity(1);
        else
            _robotStocks[key] = new RobotStockItem(robot, 1);
    }
    
    public RobotComponents GetRobotComponents(RobotBlueprint blueprint, bool simulate = false)
    {
        var core = simulate ? blueprint.CorePrototype : RemovePiece<Core>(blueprint.CorePrototype.ToString());
        var generator = simulate ? blueprint.GeneratorPrototype : RemovePiece<Generator>(blueprint.GeneratorPrototype.ToString());
        var gripModule = simulate ? blueprint.GripModulePrototype : RemovePiece<GripModule>(blueprint.GripModulePrototype.ToString());
        var moveModule = simulate ? blueprint.MoveModulePrototype : RemovePiece<MoveModule>(blueprint.MoveModulePrototype.ToString());

        return new RobotComponents(core, generator, gripModule, moveModule);
    }
    

    public IReadOnlyDictionary<string, StockItem> GetPieceStock => _stock;
    public IReadOnlyDictionary<string, RobotStockItem> GetRobotStocks => _robotStocks;
}