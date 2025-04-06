using Xunit;
using DPRobots.Pieces;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.Tests.Robots;

public class RobotBuildTests
{
    private static readonly Dictionary<string, StockItem> Stock;
    private static readonly StockManager StockManager;
    private static readonly System SystemToInstall;
    
    static RobotBuildTests()
    {
        Stock = new Dictionary<string, StockItem>
        {
            { "Core_CD1", new StockItem(new Core(CoreNames.Cd1), 5) },
            { "Core_CM1", new StockItem(new Core(CoreNames.Cm1), 5) },
            { "Core_CI1", new StockItem(new Core(CoreNames.Ci1), 5) },
            { "Generator_GD1", new StockItem(new Generator(GeneratorNames.Gd1), 5) },
            { "Generator_GM1", new StockItem(new Generator(GeneratorNames.Gm1), 5) },
            { "Generator_GI1", new StockItem(new Generator(GeneratorNames.Gi1), 5) },
            { "Arms_AM1", new StockItem(new GripModule(GripModuleNames.Am1), 5) },
            { "Arms_AD1", new StockItem(new GripModule(GripModuleNames.Ad1), 5) },
            { "Arms_AI1", new StockItem(new GripModule(GripModuleNames.Ai1), 5) },
            { "Legs_LM1", new StockItem(new MoveModule(MoveModuleNames.Lm1), 5) },
            { "Legs_LD1", new StockItem(new MoveModule(MoveModuleNames.Ld1), 5) },
            { "Legs_LI1", new StockItem(new MoveModule(MoveModuleNames.Li1), 5) },
        };
        StockManager = new StockManager(Stock);
        SystemToInstall = new System(SystemNames.Sb1);
    }
    
    [Fact]
    public void Should_BuildRobot_And_UpdateStock_When_ValidBlueprintProvided()
    {
        var robot = new Rd1();
        const string expectedName = "RD-1";
        var expectedCore = new Core(CoreNames.Cd1);
        var expectedGenerator = new Generator(GeneratorNames.Gd1);
        var expectedGripModule = new GripModule(GripModuleNames.Ad1);
        var expectedMoveModule = new MoveModule(MoveModuleNames.Ld1);

        robot.Build(StockManager, SystemToInstall);

        Assert.Equal(expectedName, robot.ToString());
        Assert.Equal(expectedCore, robot.Core);
        Assert.Equal(expectedGenerator, robot.Generator);
        Assert.Equal(expectedGripModule, robot.GripModule);
        Assert.Equal(expectedMoveModule, robot.MoveModule);
        
        Assert.Equal(4, Stock[expectedCore.ToString()].Quantity);
        Assert.Equal(4, Stock[expectedGenerator.ToString()].Quantity);
        Assert.Equal(4, Stock[expectedGripModule.ToString()].Quantity);
        Assert.Equal(4, Stock[expectedMoveModule.ToString()].Quantity);
        Assert.Equal(1, StockManager.GetRobotStocks[robot.ToString()].Quantity);
    }
}