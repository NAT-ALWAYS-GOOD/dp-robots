using DPRobots.Pieces;

namespace DPRobots.Robots;

public abstract class Robot
{
    protected abstract string Name { get; }
    public abstract RobotBlueprint Blueprint { get; }

    public Core? Core { get; private set; }
    public Generator? Generator { get; private set; }
    public GripModule? GripModule { get; private set; }
    public MoveModule? MoveModule { get; private set; }

    public void Build(RobotComponents robotComponents, System systemToInstall, bool simulate = false)
    {
        Console.WriteLine("PRODUCING " + Name);

        Console.WriteLine("GET_OUT_STOCK 1 " + robotComponents.Core);
        Console.WriteLine("GET_OUT_STOCK 1 " + robotComponents.Generator);
        Console.WriteLine("GET_OUT_STOCK 1 " + robotComponents.GripModule);
        Console.WriteLine("GET_OUT_STOCK 1 " + robotComponents.MoveModule);

        if (!simulate) robotComponents.Core.InstallSystem(systemToInstall);
        Console.WriteLine("INSTALL " + systemToInstall + " " + robotComponents.Core);

        var assemblyTmp1 = new AssembledPiece([robotComponents.Core, robotComponents.Generator], "TMP1");
        if (!simulate)
        {
            Core = robotComponents.Core;
            Generator = robotComponents.Generator;
        }

        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + robotComponents.Core + " " + robotComponents.Generator);

        var assemblyTmp2 = new AssembledPiece([assemblyTmp1, robotComponents.GripModule]);
        if (!simulate) GripModule = robotComponents.GripModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp1 + " " + robotComponents.GripModule);

        var assemblyTmp3 = new AssembledPiece([assemblyTmp2, robotComponents.MoveModule], "TMP3");
        if (!simulate) MoveModule = robotComponents.MoveModule;
        Console.WriteLine("ASSEMBLE " + assemblyTmp3 + " " + assemblyTmp2 + " " + robotComponents.MoveModule);

        Console.WriteLine("FINISHED " + Name);
    }

    public override string ToString() => Name;
}