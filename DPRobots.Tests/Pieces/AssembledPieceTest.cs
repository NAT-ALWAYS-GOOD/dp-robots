using DPRobots.Pieces;
using Xunit;

namespace DPRobots.Tests.Pieces;

public class AssembledPieceTest
{
    [Fact]
    public void Should_Create_AssembledPiece()
    {
        var core = (Core)PieceFactory.Create("Core_CM1");
        var system = (System)PieceFactory.Create("System_SB1");
        core.InstallSystem(system);
        
        var generator = (Generator)PieceFactory.Create("Generator_GM1");

        var tmp1 = new AssembledPiece([core, generator], "TMP1");
        
        Assert.Equal("TMP1", tmp1.ToString());
    }
    
    [Fact]
    public void Should_Create_A_Computedly_Named_AssembledPiece()
    {
        var core = (Core)PieceFactory.Create("Core_CM1");
        var system = (System)PieceFactory.Create("System_SB1");
        core.InstallSystem(system);
        
        var generator = (Generator)PieceFactory.Create("Generator_GM1");

        var assembledPiece = new AssembledPiece([core, generator]);
        
        Assert.Equal("[Core_CM1,Generator_GM1]", assembledPiece.ToString());
    }
    
    [Fact]
    public void Should_Create_A_Computedly_Named_AssembledPiece_With_AssembledPieces()
    {
        var core = (Core)PieceFactory.Create("Core_CM1");
        var system = (System)PieceFactory.Create("System_SM1");
        core.InstallSystem(system);
        
        var generator = (Generator)PieceFactory.Create("Generator_GM1");

        var firstAssembledPiece = new AssembledPiece([core, generator]);
        
        var moveModule = (MoveModule)PieceFactory.Create("Legs_LD1");
        
        var secondAssembledPiece = new AssembledPiece([firstAssembledPiece, moveModule]);
        
        Assert.Equal("[[Core_CM1,Generator_GM1],Legs_LD1]", secondAssembledPiece.ToString());
    }
    
    [Fact]
    public void Should_Throw_ArgumentException_When_AssembledPieces_Is_Empty()
    {
        var exception = Assert.Throws<ArgumentException>(() => new AssembledPiece([]));
        Assert.Equal("Pieces cannot be null or empty", exception.Message);
    }
}