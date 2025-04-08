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

    public void Build(StockManager stockManager, System systemToInstall, bool simulate = false)
    {
        Console.WriteLine("PRODUCING " + Name);

        Core neededCore;
        Generator neededGenerator;
        GripModule neededGripModule;
        MoveModule neededMoveModule;

        if (simulate)
        {
            neededCore = Blueprint.CorePrototype;
            neededGenerator = Blueprint.GeneratorPrototype;
            neededGripModule = Blueprint.GripModulePrototype;
            neededMoveModule = Blueprint.MoveModulePrototype;
        }
        else
        {
            neededCore = stockManager.RemovePiece<Core>(Blueprint.CorePrototype.ToString());
            neededGenerator = stockManager.RemovePiece<Generator>(Blueprint.GeneratorPrototype.ToString());
            neededGripModule = stockManager.RemovePiece<GripModule>(Blueprint.GripModulePrototype.ToString());
            neededMoveModule = stockManager.RemovePiece<MoveModule>(Blueprint.MoveModulePrototype.ToString());
        }

        Console.WriteLine("GET_OUT_STOCK 1 " + neededCore);
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGenerator);
        Console.WriteLine("GET_OUT_STOCK 1 " + neededGripModule);
        Console.WriteLine("GET_OUT_STOCK 1 " + neededMoveModule);

        if (!simulate) neededCore.InstallSystem(systemToInstall);
        Console.WriteLine("INSTALL " + systemToInstall + " " + neededCore);

        var assemblyTmp1 = new AssembledPiece([neededCore, neededGenerator], "TMP1");
        if (!simulate)
        {
            Core = neededCore;
            Generator = neededGenerator;
        }

        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + neededCore + " " + neededGenerator);

        var assemblyTmp2 = new AssembledPiece([assemblyTmp1, neededGripModule]);
        if (!simulate) GripModule = neededGripModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + neededGripModule);

        var assemblyTmp3 = new AssembledPiece([assemblyTmp2, neededMoveModule], "TMP3");
        if (!simulate) MoveModule = neededMoveModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp3 + " " + assemblyTmp2 + " " + neededMoveModule);

        Console.WriteLine("FINISHED " + Name);
        if (!simulate) stockManager.AddRobot(this);
    }

    public override string ToString() => Name;
}