using DPRobots.Pieces;
using DPRobots.RobotFactories;
using DPRobots.Robots;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests.Robots;

public class RobotBuilderTests
{
    private readonly RobotFactory _factory = new("Usine");

    public RobotBuilderTests()
    {
        _factory.Templates.InitializeTemplates();
    }

    [Fact]
    public void Should_Build_Robot_With_Correct_Components()
    {
        _factory.Stock.Initialize([
            new StockItem(PieceFactory.Create("Core_CD1"), 10),
            new StockItem(PieceFactory.Create("Generator_GD1"), 10),
            new StockItem(PieceFactory.Create("Arms_AD1"), 10),
            new StockItem(PieceFactory.Create("Legs_LD1"), 10)
        ]);
        var robot = new RobotBuilder("RD-1", _factory)
            .UseTemplate()
            .Build();

        Assert.NotNull(robot.Core);
        Assert.NotNull(robot.Generator);
        Assert.NotNull(robot.GripModule);
        Assert.NotNull(robot.MoveModule);
        Assert.Equal("RD-1", robot.ToString());
    }

    [Fact]
    public void Should_Throw_If_Template_Invalid_Name()
    {
        var builder = new RobotBuilder("ZOMBIE-9000", _factory);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            builder.UseTemplate());

        Assert.Equal("`ZOMBIE-9000` is not a recognized robot", ex.Message);
    }
}