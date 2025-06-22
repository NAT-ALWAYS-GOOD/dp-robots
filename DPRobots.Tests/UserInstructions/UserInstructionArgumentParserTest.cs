using DPRobots.Pieces;
using DPRobots.RobotFactories;
using DPRobots.Robots;
using DPRobots.UserInstructions;
using Xunit;

namespace DPRobots.Tests.UserInstructions;

public class UserUserInstructionArgumentParserTest
{
    private readonly RobotFactory _factory = new ("Usine");
    public UserUserInstructionArgumentParserTest()
    {
        _factory.Templates.InitializeTemplates();
    }
    
    [Fact]
    public void Should_Parse_RobotCounts_Correctly()
    {
        var result = UserInstructionArgumentParser.ParseRobotsWithQuantities("2 XM-1, 1 RD-1", _factory);

        Assert.Equal(2, result[_factory.Templates.Get("XM-1")!]);
        Assert.Equal(1, result[_factory.Templates.Get("RD-1")!]);
    }

    [Fact]
    public void Should_Throw_If_Format_Is_Invalid()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            UserInstructionArgumentParser.ParseRobotsWithQuantities("XM-1", _factory));

        Assert.Contains("Le format de l'argument 'XM-1' est incorrect. Attendu : 'quantité nom_de_robot'.", ex.Message);
    }

    [Fact]
    public void Should_Throw_If_Quantity_Is_Invalid()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            UserInstructionArgumentParser.ParseRobotsWithQuantities("abc XM-1", _factory));

        Assert.Contains("La quantité 'abc' n'est pas un nombre valide.", ex.Message);
    }

    [Fact]
    public void Should_Sum_Quantities_If_Duplicated()
    {
        var result = UserInstructionArgumentParser.ParseRobotsWithQuantities("1 XM-1, 2 XM-1", _factory);

        Assert.Equal(3, result[_factory.Templates.Get("XM-1")!]);
    }
    
    [Fact]
    public void Should_Parse_Template_Correctly()
    {
        var args = "MYBOT, Core_CM1, System_SB1, Generator_GM1, Arms_AM1, Legs_LM1";
        var blueprint = UserInstructionArgumentParser.ParseRobotBlueprint(args);

        Assert.Equal("MYBOT", blueprint.Name);
        Assert.IsType<Core>(blueprint.CorePrototype);
        Assert.IsType<System>(blueprint.SystemPrototype);
        Assert.IsType<Generator>(blueprint.GeneratorPrototype);
        Assert.IsType<GripModule>(blueprint.GripModulePrototype);
        Assert.IsType<MoveModule>(blueprint.MoveModulePrototype);
    }

    [Fact]
    public void Should_Throw_If_Not_Enough_Pieces()
    {
        var args = "MYBOT, Core_CM1, System_SB1, Generator_GM1";

        var ex = Assert.Throws<ArgumentException>(() =>
            UserInstructionArgumentParser.ParseRobotBlueprint(args));

        Assert.Contains("Il manque une ou plusieurs pièces requises.", ex.Message);
    }

    [Fact]
    public void Should_Throw_If_Duplicated_Piece_Types()
    {
        var args = "MYBOT, Core_CM1, Core_CD1, System_SB1, Generator_GM1, Arms_AM1, Legs_LM1";

        var ex = Assert.Throws<ArgumentException>(() =>
            UserInstructionArgumentParser.ParseRobotBlueprint(args));

        Assert.Contains("dupliquée", ex.Message);
    }

    [Fact]
    public void Should_Throw_If_Piece_Type_Is_Unrecognized()
    {
        var args = "MYBOT, UnknownPiece, System_SB1, Generator_GM1, Arms_AM1, Legs_LM1";

        var ex = Assert.Throws<ArgumentException>(() =>
            UserInstructionArgumentParser.ParseRobotBlueprint(args));

        Assert.Contains("UnknownPiece", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}