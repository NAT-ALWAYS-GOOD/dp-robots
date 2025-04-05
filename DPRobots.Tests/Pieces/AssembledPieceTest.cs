using DPRobots.Pieces;
using Xunit;

namespace DPRobots.Tests.Pieces;

public class AssembledPieceTest
{
    [Fact]
    public void Should_Create_AssembledPiece()
    {
        var core = new Core(CoreNames.Cm1);
        var system = new System(SystemNames.Sb1);
        core.InstallSystem(system);
        
        var generator = new Generator(GeneratorNames.Gm1);

        var tmp1 = new AssembledPiece([core, generator], "TMP1");
        
        Assert.Equal("TMP1", tmp1.ToString());
    }
    
    [Fact]
    public void Should_Create_A_Computedly_Named_AssembledPiece()
    {
        var core = new Core(CoreNames.Cm1);
        var system = new System(SystemNames.Sb1);
        core.InstallSystem(system);
        
        var generator = new Generator(GeneratorNames.Gm1);

        var assembledPiece = new AssembledPiece([core, generator]);
        
        Assert.Equal("[Core_CM1,Generator_GM1]", assembledPiece.ToString());
    }
    
    [Fact]
    public void Should_Create_A_Computedly_Named_AssembledPiece_With_AssembledPieces()
    {
        var core = new Core(CoreNames.Cm1);
        var system = new System(SystemNames.Sb1);
        core.InstallSystem(system);
        
        var generator = new Generator(GeneratorNames.Gm1);

        var firstAssembledPiece = new AssembledPiece([core, generator]);
        
        var moveModule = new MoveModule(MoveModuleNames.Ld1);
        
        var secondAssembledPiece = new AssembledPiece([firstAssembledPiece, moveModule]);
        
        Assert.Equal("[Core_CM1,Generator_GM1,Legs_LD1]", secondAssembledPiece.ToString());
    }
    
    [Fact]
    public void Should_Throw_ArgumentException_When_AssembledPieces_Is_Empty()
    {
        var exception = Assert.Throws<ArgumentException>(() => new AssembledPiece([]));
        Assert.Equal("Pieces cannot be null or empty", exception.Message);
    }
}