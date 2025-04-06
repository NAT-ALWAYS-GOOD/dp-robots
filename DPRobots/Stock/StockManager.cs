using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager(Dictionary<string, StockItem> stock)
{
    private readonly Dictionary<string, StockItem> _stocks = stock;
    private readonly Dictionary<string, RobotStockItem> _robotStocks = new Dictionary<string, RobotStockItem>();

    public static StockManager? Instance { get; private set; }

    public static void Initialize(Dictionary<string, StockItem> stock)
    {
        Instance = new StockManager(stock);
    }

    public T RemovePiece<T>(string key) where T : Piece
    {
        var stockItem = _stocks.GetValueOrDefault(key);
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

    public IReadOnlyDictionary<string, StockItem> GetPieceStock => _stocks;
    public IReadOnlyDictionary<string, RobotStockItem> GetRobotStocks => _robotStocks;
}