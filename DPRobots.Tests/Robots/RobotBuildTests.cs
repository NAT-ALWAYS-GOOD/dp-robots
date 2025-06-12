using Xunit;
using DPRobots.Pieces;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.Tests.Robots;

public class RobotBuildTests
{
    private static readonly StockManager StockManager;
    private static readonly System SystemToInstall;

    static RobotBuildTests()
    {
        List<StockItem> stock =
        [
            new(new Core(CoreNames.Cd1, PieceCategory.Domestic), 5),
            new(new Core(CoreNames.Cm1, PieceCategory.Military), 5),
            new(new Core(CoreNames.Ci1, PieceCategory.Industrial), 5),
            new(new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), 5),
            new(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 5),
            new(new Generator(GeneratorNames.Gi1, PieceCategory.Industrial), 5),
            new(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 5),
            new(new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), 5),
            new(new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial), 5),
            new(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 5),
            new(new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic), 5),
            new(new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial), 5)
        ];
        StockManager = StockManager.GetInstance();
        StockManager.Initialize(stock);
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

        Assert.Equal(4,
            StockManager.GetPieceStocks
                .Where(stockItem => stockItem.Prototype.Equals(expectedCore))
                .First()
                .Quantity
        );
        Assert.Equal(4,
            StockManager.GetPieceStocks
                .Where(stockItem => stockItem.Prototype.Equals(expectedGenerator))
                .First()
                .Quantity
        );
        Assert.Equal(4,
            StockManager.GetPieceStocks
                .Where(stockItem => stockItem.Prototype.Equals(expectedGripModule))
                .First()
                .Quantity);
        Assert.Equal(4,
            StockManager.GetPieceStocks
                .Where(stockItem => stockItem.Prototype.Equals(expectedMoveModule))
                .First()
                .Quantity);
        Assert.Equal(1,
            StockManager.GetRobotStocks
                .Where(stockItem => stockItem.RobotPrototype.Equals(robot))
                .First()
                .Quantity
        );
    }
}