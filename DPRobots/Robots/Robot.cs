using DPRobots.Pieces;
using DPRobots.Stock;

namespace DPRobots.Robots;

public abstract class Robot
{
    protected abstract string Name { get; }
    protected abstract RobotBlueprint Blueprint { get; }
    
    public Core? Core { get; set; }
    public Generator? Generator { get; set; }
    public GripModule? GripModule { get; set; }
    public MoveModule? MoveModule { get; set; }

    public void Build(StockManager stockManager, System systemToInstall)
    {
        Console.WriteLine("PRODUCING " + Name);

        var neededCore = stockManager.RemovePiece<Core>(Blueprint.CorePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededCore.ToString());

        var neededGenerator = stockManager.RemovePiece<Generator>(Blueprint.GeneratorPrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGenerator.ToString());

        var neededGripModule = stockManager.RemovePiece<GripModule>(Blueprint.GripModulePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGripModule.ToString());

        var neededMoveModule = stockManager.RemovePiece<MoveModule>(Blueprint.MoveModulePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededMoveModule.ToString());

        neededCore.InstallSystem(systemToInstall);
        Console.WriteLine("INSTALL " + systemToInstall.ToString() + " " + neededCore.ToString());

        var assemblyTmp1 = new AssembledPiece(new List<Piece> { neededCore, neededGenerator }, "TMP1");
        Core = neededCore;
        Generator = neededGenerator;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1.ToString() + " " + Core.ToString() + " " + Generator.ToString());

        var assemblyTmp2 = new AssembledPiece(new List<Piece> { assemblyTmp1, neededGripModule });
        GripModule = neededGripModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1.ToString() + " " + GripModule.ToString());

        var assemblyTmp3 = new AssembledPiece(new List<Piece> { assemblyTmp2, neededMoveModule }, "TMP3");
        MoveModule = neededMoveModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp3.ToString() + " " + assemblyTmp2.ToString() + " " +
                          MoveModule.ToString());

        Console.WriteLine("FINISHED " + Name);
    }
    
    public override string ToString() => Name;
}