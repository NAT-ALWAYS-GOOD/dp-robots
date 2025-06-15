using DPRobots.Instructions;
using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.Stock;

namespace DPRobots.Robots;

public class RobotBuilder(string name)
{
    private RobotBlueprint? _blueprint;
    private List<IInstruction> _instructions = [];

    public RobotBuilder UseTemplate()
    {
        _blueprint = RobotTemplates.Get(name);
        if (_blueprint is null) Logger.Log(LogType.ERROR, $"`{name}` is not a recognized robot");
        return this;
    }

    public RobotBuilder GenerateInstructions(
        Core? core = null,
        Generator? generator = null,
        GripModule? gripModule = null,
        MoveModule? moveModule = null,
        System? system = null)
    {
        if (_blueprint is null)
            throw new InvalidOperationException("Template must be set before generating instructions.");

        Core notNullCore = core ?? _blueprint.CorePrototype;
        Generator notNullGenerator = generator ?? _blueprint.GeneratorPrototype;
        GripModule notNullGripModule = gripModule ?? _blueprint.GripModulePrototype;
        MoveModule notNullMoveModule = moveModule ?? _blueprint.MoveModulePrototype;
        System notNullSystem = system ?? _blueprint.SystemPrototype;

        var tmp1 = new AssembledPiece([notNullCore, notNullGenerator], "TMP1");

        _instructions = new List<IInstruction>
        {
            new ProduceInstruction(name),
            new GetOutStockInstruction(notNullCore),
            new GetOutStockInstruction(notNullGenerator),
            new GetOutStockInstruction(notNullGripModule),
            new GetOutStockInstruction(notNullMoveModule),
            new InstallSystemInstruction(notNullSystem, notNullCore),
            new AssembleInstruction("TMP1", notNullCore, notNullGenerator),
            new AssembleInstruction(
                null,
                tmp1,
                notNullGripModule
            ),
            new AssembleInstruction(
                "TMP3",
                new AssembledPiece([
                    tmp1,
                    notNullGripModule,
                ]),
                notNullMoveModule
            ),
            new FinishInstruction(name),
        };

        return this;
    }

    public void Simulate()
    {
        foreach (var instruction in _instructions) Console.WriteLine(instruction);
    }

    public Robot Build(bool? printInstructions = true)
    {
        if (_blueprint is null)
            throw new InvalidOperationException("Template must be provided before building.");

        if (printInstructions == true)
        {
            foreach (var instruction in _instructions) Console.WriteLine(instruction);
        }

        var robot = new Robot(name, blueprint: _blueprint);
        var robotComponents = StockManager.GetRobotComponents(robot.Blueprint);
        var core = robotComponents.Core;
        core.InstallSystem(_blueprint.SystemPrototype);
        robot.Core = core;
        robot.Generator = robotComponents.Generator;
        robot.GripModule = robotComponents.GripModule;
        robot.MoveModule = robotComponents.MoveModule;

        return robot;
    }
}