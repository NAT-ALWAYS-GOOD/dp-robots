using Xunit;
using DPRobots.Stock;
using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Tests.Stock;
public class StockManagerTests
{
    [Fact]
    public void Initialize_ShouldAddItemsToStock()
    {
        var stock = new StockManager();
        var item = new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 10);

        stock.Initialize(new List<StockItem> { item });

        var result = stock.GetStock.Single();
        Assert.Equal("Core_CM1", result.Prototype.ToString());
        Assert.Equal(10, result.Quantity);
    }

    [Fact]
    public void Has_ShouldReturnTrueIfSufficientQuantity()
    {
        var stock = new StockManager();
        stock.Initialize(new List<StockItem> { new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 5) });

        var result = stock.Has(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 3));
        Assert.True(result);
    }

    [Fact]
    public void Has_ShouldReturnFalseIfInsufficientQuantity()
    {
        var stock = new StockManager();
        stock.Initialize(new List<StockItem> { new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 2) });

        var result = stock.Has(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 5));
        Assert.False(result);
    }

    [Fact]
    public void AddStockItem_ShouldIncreaseQuantity()
    {
        var stock = new StockManager();
        stock.Initialize(new List<StockItem> { new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 2) });

        stock.AddStockItem(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 3));

        var result = stock.GetStock.Single();
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public void RemoveStockItem_ShouldDecreaseQuantity()
    {
        var stock = new StockManager();
        stock.Initialize(new List<StockItem> { new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 5) });

        stock.RemoveStockItem(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 3));

        var result = stock.GetStock.Single();
        Assert.Equal(2, result.Quantity);
    }

    [Fact]
    public void RemovePiece_ShouldRemoveAndReturnClone()
    {
        var stock = new StockManager();
        var core = new Core(CoreNames.Cm1, PieceCategory.Military);
        stock.Initialize(new List<StockItem> { new StockItem(core, 1) });

        var removed = stock.RemovePiece<Core>(core);

        Assert.Equal(core.ToString(), removed.ToString());
        Assert.Empty(stock.GetStock);
    }

    [Fact]
    public void AddRobot_ShouldIncreaseRobotQuantity()
    {
        var stock = new StockManager();
        var robot = new Robot("RD-1", new RobotBlueprint("RD-1", new Core(CoreNames.Cd1, PieceCategory.Domestic), new System(SystemNames.Sb1, PieceCategory.Domestic), new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic)));

        stock.AddRobot(robot, 2);

        var item = stock.GetStock.Single();
        Assert.Equal(2, item.Quantity);
        Assert.Equal(robot.ToString(), ((Robot)item.Prototype).ToString());
    }

    [Fact]
    public void GetRobotComponents_ShouldRemoveCorrectPieces()
    {
        var stock = new StockManager();
        var blueprint = new RobotBlueprint("RD-1",
            new Core(CoreNames.Cd1, PieceCategory.Domestic),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gd1, PieceCategory.Domestic),
            new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic),
            new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic));

        
        stock.Initialize(new List<StockItem>
        {
            new(new Core(CoreNames.Cd1, PieceCategory.Domestic), 1),
            new(new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), 1),
            new(new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), 1),
            new(new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic), 1)
        });

        var components = stock.GetRobotComponents(blueprint);

        Assert.Equal("Core_CD1", components.Core.ToString());
        Assert.Empty(stock.GetStock);
    }

    [Fact]
    public void CalculateOverallNeededStocks_ShouldAggregateQuantities()
    {
        var blueprint = new RobotBlueprint("RD-1",
            new Core(CoreNames.Cd1, PieceCategory.Domestic),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gd1, PieceCategory.Domestic),
            new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic),
            new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic));

        var result = StockManager.CalculateOverallNeededStocks(
            new Dictionary<RobotBlueprint, int> { [blueprint] = 2 });

        Assert.Equal(2, result[new Core(CoreNames.Cd1, PieceCategory.Domestic)]);
        Assert.Equal(2, result[new Generator(GeneratorNames.Gd1, PieceCategory.Domestic)]);
        Assert.Equal(2, result[new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic)]);
        Assert.Equal(2, result[new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic)]);
    }

    [Fact]
    public void VerifyRequestedQuantitiesAreAvailable_ShouldReturnFalseIfInsufficientStock()
    {
        var stock = new StockManager();
        var blueprint = new RobotBlueprint("RD-1",
            new Core(CoreNames.Cd1, PieceCategory.Domestic),
            new System(SystemNames.Sb1, PieceCategory.General),
            new Generator(GeneratorNames.Gd1, PieceCategory.Domestic),
            new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic),
            new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic));

        stock.Initialize(new List<StockItem>
        {
            new(new Core(CoreNames.Cm1, PieceCategory.Military), 1),
            new(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 1),
            new(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 1),
            new(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 1)
        });

        var isAvailable = stock.VerifyRequestedQuantitiesAreAvailable(
            new Dictionary<RobotBlueprint, int> { [blueprint] = 2 });

        Assert.False(isAvailable);
    }

    [Fact]
    public void GetMovements_ShouldFilterCorrectly()
    {
        var stock = new StockManager();
        stock.AddStockItem(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 2), context: "Init");
        stock.RemoveStockItem(new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 1), context: "Used");

        var all = stock.GetMovements().ToList();
        var filtered = stock.GetMovements("C1").ToList();

        Assert.Equal(2, all.Count);
        Assert.All(filtered, m => Assert.Equal("C1", m.ItemName));
    }
}
