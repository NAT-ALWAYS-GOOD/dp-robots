using DPRobots.Pieces;
using Xunit;

namespace DPRobots.Tests.Pieces;

public class PieceFactoryTests
{
    [Theory]
    [InlineData("Core_CM1", typeof(Core))]
    [InlineData("Generator_GM1", typeof(Generator))]
    [InlineData("Arms_AM1", typeof(GripModule))]
    [InlineData("Legs_LM1", typeof(MoveModule))]
    [InlineData("System_SB1", typeof(System))]
    public void Should_Create_Correct_Piece_Type(string name, Type expectedType)
    {
        var piece = PieceFactory.Create(name);
        Assert.NotNull(piece);
        Assert.IsType(expectedType, piece);
    }

    [Theory]
    [InlineData("Core_CM1", PieceCategory.Military)]
    [InlineData("Generator_GD1", PieceCategory.Domestic)]
    [InlineData("System_SB1", PieceCategory.General)]
    public void Should_Return_Correct_Category(string name, PieceCategory expectedCategory)
    {
        var piece = PieceFactory.Create(name);
        Assert.Equal(expectedCategory, piece.Category);
    }

    [Fact]
    public void Should_Throw_If_Piece_Not_Known()
    {
        var ex = Assert.Throws<ArgumentException>(() => PieceFactory.Create("Unknown_123"));
        Assert.Equal("No piece named 'Unknown_123'", ex.Message);
    }
}