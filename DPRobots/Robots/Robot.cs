using DPRobots.Instructions;
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
    
    /// <summary>
    /// Récupère le robot correspondant au nom donné.
    /// Si le nom ne correspond à aucun robot connu, retourne null.
    /// </summary>
    /// <param name="robotName"></param>
    public static Robot? FromName(string robotName)
    {
        return robotName.ToUpper() switch
        {
            "XM-1" => new Xm1(),
            "RD-1" => new Rd1(),
            "WI-1" => new Wi1(),
            _ => null
        };
    }

    public void Build(RobotComponents robotComponents, System systemToInstall, bool simulate = false)
    {
        var assemblyTmp1 = new AssembledPiece([robotComponents.Core, robotComponents.Generator], "TMP1");
        var instructionsList = new List<IInstruction>
        {
            new ProduceInstruction(Name),
            new GetOutStockInstruction(robotComponents.Core),
            new GetOutStockInstruction(robotComponents.Generator),
            new GetOutStockInstruction(robotComponents.GripModule),
            new GetOutStockInstruction(robotComponents.MoveModule),
            new InstallSystemInstruction(systemToInstall, robotComponents.Core),
            new AssembleInstruction("TMP1", robotComponents.Core, robotComponents.Generator),
            new AssembleInstruction(
                null,
                assemblyTmp1,
                robotComponents.GripModule
            ),
            new AssembleInstruction(
                "TMP3",
                new AssembledPiece([
                    assemblyTmp1,
                    robotComponents.GripModule,
                ]),
                robotComponents.MoveModule
            ),
            new FinishInstruction(Name),
        };

        foreach (var instruction in instructionsList) Console.WriteLine(instruction);

        if (simulate) return;
        robotComponents.Core.InstallSystem(systemToInstall);
        Core = robotComponents.Core;
        Generator = robotComponents.Generator;
        GripModule = robotComponents.GripModule;
        MoveModule = robotComponents.MoveModule;
    }
    
    public List<Piece> GetNeededPieces()
    {
        return
        [
            Blueprint.CorePrototype,
            Blueprint.GeneratorPrototype,
            Blueprint.GripModulePrototype,
            Blueprint.MoveModulePrototype
        ];
    }

    public override string ToString() => Name;
}