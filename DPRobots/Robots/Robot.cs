using DPRobots.Pieces;
using DPRobots.Stock;

namespace DPRobots.Robots;

public abstract class Robot
{
    protected abstract string Name { get; }
    public abstract RobotBlueprint Blueprint { get; }

    public Core? Core { get; private set; }
    public Generator? Generator { get; private set; }
    public GripModule? GripModule { get; private set; }
    public MoveModule? MoveModule { get; private set; }

    public void Build(StockManager stockManager, System systemToInstall)
    {
        Console.WriteLine("PRODUCING " + Name);

        var neededCore = stockManager.RemovePiece<Core>(Blueprint.CorePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededCore);

        var neededGenerator = stockManager.RemovePiece<Generator>(Blueprint.GeneratorPrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGenerator);

        var neededGripModule = stockManager.RemovePiece<GripModule>(Blueprint.GripModulePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGripModule);

        var neededMoveModule = stockManager.RemovePiece<MoveModule>(Blueprint.MoveModulePrototype.ToString());
        Console.WriteLine("GET_OUT_STOCK 1 " + neededMoveModule);

        neededCore.InstallSystem(systemToInstall);
        Console.WriteLine("INSTALL " + systemToInstall + " " + neededCore);

        var assemblyTmp1 = new AssembledPiece([neededCore, neededGenerator], "TMP1");
        Core = neededCore;
        Generator = neededGenerator;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + Core + " " + Generator);

        var assemblyTmp2 = new AssembledPiece([assemblyTmp1, neededGripModule]);
        GripModule = neededGripModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + GripModule);

        var assemblyTmp3 = new AssembledPiece([assemblyTmp2, neededMoveModule], "TMP3");
        MoveModule = neededMoveModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp3 + " " + assemblyTmp2 + " " + MoveModule);

        Console.WriteLine("FINISHED " + Name);
        stockManager.AddRobot(this);
    }

    public override string ToString() => Name;
}