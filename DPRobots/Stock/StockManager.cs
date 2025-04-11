using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Stock;

public class StockManager(Dictionary<string, StockItem> stock)
{
    private readonly Dictionary<string, RobotStockItem> _robotStocks = new();

    private static StockManager? _instance;

    public static StockManager GetInstance(Dictionary<string, StockItem>? stock = null)
    {
        if (_instance != null)
            return _instance;

        return _instance = new StockManager(stock ?? new Dictionary<string, StockItem>());
    }

    public T RemovePiece<T>(string key) where T : Piece
    {
        var stockItem = stock.GetValueOrDefault(key);
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

    public IReadOnlyDictionary<string, StockItem> GetPieceStock => stock;
    public IReadOnlyDictionary<string, RobotStockItem> GetRobotStocks => _robotStocks;
}