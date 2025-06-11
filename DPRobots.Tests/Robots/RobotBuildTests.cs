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
            { "Core_CD1", new StockItem(new Core(CoreNames.Cd1, PieceCategory.Domestic), 5) },
            { "Core_CM1", new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 5) },
            { "Core_CI1", new StockItem(new Core(CoreNames.Ci1, PieceCategory.Industrial), 5) },
            { "Generator_GD1", new StockItem(new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), 5) },
            { "Generator_GM1", new StockItem(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 5) },
            { "Generator_GI1", new StockItem(new Generator(GeneratorNames.Gi1, PieceCategory.Industrial), 5) },
            { "Arms_AM1", new StockItem(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 5) },
            { "Arms_AD1", new StockItem(new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), 5) },
            { "Arms_AI1", new StockItem(new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial), 5) },
            { "Legs_LM1", new StockItem(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 5) },
            { "Legs_LD1", new StockItem(new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic), 5) },
            { "Legs_LI1", new StockItem(new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial), 5) },
        };
        StockManager = StockManager.GetInstance();
        StockManager.Initialize(Stock);
        SystemToInstall = new System(SystemNames.Sb1, PieceCategory.General);
    }

    [Fact]
    public void Should_BuildRobot_And_UpdateStock_When_ValidBlueprintProvided()
    {
        var robot = new Rd1();
        const string expectedName = "RD-1";

        var expectedCore = new Core(CoreNames.Cd1, PieceCategory.Domestic);
        var expectedGenerator = new Generator(GeneratorNames.Gd1, PieceCategory.Domestic);
        var expectedGripModule = new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic);
        var expectedMoveModule = new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic);

        var robotComponents = StockManager.GetRobotComponents(robot.Blueprint);

        robot.Build(robotComponents, SystemToInstall);
        StockManager.AddRobot(robot);

        Assert.Equal(expectedName, robot.ToString());
        Assert.Equal(expectedCore, robot.Core);
        Assert.Equal(expectedGenerator, robot.Generator);
        Assert.Equal(expectedGripModule, robot.GripModule);
        Assert.Equal(expectedMoveModule, robot.MoveModule);

        Assert.Equal(4, StockManager.GetPieceStock[expectedCore.ToString()].Quantity);
        Assert.Equal(4, StockManager.GetPieceStock[expectedGenerator.ToString()].Quantity);
        Assert.Equal(4, StockManager.GetPieceStock[expectedGripModule.ToString()].Quantity);
        Assert.Equal(4, StockManager.GetPieceStock[expectedMoveModule.ToString()].Quantity);
        Assert.Equal(1, StockManager.GetRobotStocks[robot.ToString()].Quantity);
    }
}