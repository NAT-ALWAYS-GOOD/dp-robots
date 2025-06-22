using DPRobots.Stock;

namespace DPRobots.RobotFactories;

public class FactoryManager
{
    public readonly List<RobotFactory> Factories = new();
    
    private static FactoryManager? _instance;
    
    public static FactoryManager GetInstance()
    {
        if (_instance != null)
            return _instance;

        return _instance = new FactoryManager();
    }

    public void RegisterFactory(RobotFactory factory)
    {
        if (Factories.Any(f => f.Name.Equals(factory.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"Une usine avec le nom '{factory.Name}' existe déjà.");
        Factories.Add(factory);
    }

    public RobotFactory GetFactory(string name)
    {
        var factory = Factories.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (factory == null)
            throw new ArgumentException($"Aucune usine trouvée avec le nom '{name}'.");
        return factory;
    }

    public List<StockItem> GetTotalStockItems()
    {
        var totalStockItems = new List<StockItem>();
        foreach (var factory in Factories)
        {
            var toAdd = factory.Stock.GetStock;
            foreach (var item in toAdd)
            {
                var existingItem = totalStockItems.FirstOrDefault(si => si.Prototype.Equals(item.Prototype));
                if (existingItem != null)
                {
                    existingItem.IncreaseQuantity(item.Quantity);
                }
                else
                {
                    totalStockItems.Add(new StockItem(item.Prototype, item.Quantity));
                }
            }
        }
        return totalStockItems;
    }
    
    public List<RobotFactory> GetAvailableFactories(Dictionary<string, int> robotsRequest)
    {
        return Factories
            .Where(f => robotsRequest.All(r => f.Templates.Get(r.Key) is not null))
            .Where(f =>
            {
                var blueprints = robotsRequest
                    .Select(r => new { Blueprint = f.Templates.Get(r.Key)!, Quantity = r.Value })
                    .ToDictionary(x => x.Blueprint, x => x.Quantity);

                return f.Stock.VerifyRequestedQuantitiesAreAvailable(blueprints);
            })
            .ToList();
    }
}
