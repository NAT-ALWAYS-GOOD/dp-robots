using DPRobots.Pieces;
using DPRobots.Robots;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests.Robots;

public class RobotBuilderTests
{
    public RobotBuilderTests()
    {
        RobotTemplates.GetInstance().InitializeTemplates();
    }

    [Fact]
    public void Should_Use_Template_And_Generate_Instructions()
    {
        var builder = new RobotBuilder("XM-1")
            .UseTemplate()
            .GenerateInstructions();

        var simulateOutput = new StringWriter();
        Console.SetOut(simulateOutput);
        builder.Simulate();

        string output = simulateOutput.ToString();
        Assert.Contains("ASSEMBLE TMP1", output);
        Assert.Contains("FINISHED XM-1", output);
    }

    [Fact]
    public void Should_Build_Robot_With_Correct_Components()
    {
        StockManager.GetInstance().Initialize([
            new StockItem(PieceFactory.Create("Core_CD1"), 10),
            new StockItem(PieceFactory.Create("Generator_GD1"), 10),
            new StockItem(PieceFactory.Create("Arms_AD1"), 10),
            new StockItem(PieceFactory.Create("Legs_LD1"), 10)
        ]);
        var builder = new RobotBuilder("RD-1")
            .UseTemplate()
            .GenerateInstructions();

        var robot = builder.Build();

        Assert.NotNull(robot.Core);
        Assert.NotNull(robot.Generator);
        Assert.NotNull(robot.GripModule);
        Assert.NotNull(robot.MoveModule);
        Assert.Equal("RD-1", robot.ToString());
    }

    [Fact]
    public void Should_Throw_If_Template_Not_Set()
    {
        var builder = new RobotBuilder("XM-1");

        var ex = Assert.Throws<InvalidOperationException>(() =>
            builder.GenerateInstructions());

        Assert.Equal("Template must be set before generating instructions.", ex.Message);
    }

    [Fact]
    public void Should_Throw_If_Template_Invalid_Name()
    {
        var builder = new RobotBuilder("ZOMBIE-9000");
        builder.UseTemplate();

        var ex = Assert.Throws<InvalidOperationException>(() =>
            builder.GenerateInstructions());

        Assert.Equal("Template must be set before generating instructions.", ex.Message);
    }

    [Fact]
    public void Should_Allow_Overriding_Parts()
    {
        var core = new Core(CoreNames.Cm1, PieceCategory.Military);
        var generator = new Generator(GeneratorNames.Gm1, PieceCategory.Military);

        var builder = new RobotBuilder("XM-1")
            .UseTemplate()
            .GenerateInstructions(core: core, generator: generator);

        var simulateOutput = new StringWriter();
        Console.SetOut(simulateOutput);
        builder.Simulate();

        string output = simulateOutput.ToString();
        Assert.Contains("Core_CM1", output);
        Assert.Contains("Generator_GM1", output);
    }
}