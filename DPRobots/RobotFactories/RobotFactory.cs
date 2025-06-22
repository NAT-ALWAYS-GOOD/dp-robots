using DPRobots.Stock;

namespace DPRobots.RobotFactories;

public class RobotFactory(string name)
{
    public string Name { get; } = name;
    public StockManager Stock { get; } = new();
    public override string ToString() => Name;
    
    public RobotFactory(String name, List<StockItem> initialStock) : this(name)
    {
        Stock.Initialize(initialStock);
    }
}
