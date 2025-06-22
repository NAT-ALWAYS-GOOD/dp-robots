using DPRobots.Pieces;
using DPRobots.Robots;

namespace DPRobots.Instructions;

public class InstructionsGenerator
{
    private static InstructionsGenerator? _instance;
    
    public static InstructionsGenerator GetInstance()
    {
        if (_instance != null)
            return _instance;

        return _instance = new InstructionsGenerator();
    }
    
    public List<IInstruction> GenerateInstructions(RobotBlueprint blueprint)
    {
        Core core = blueprint.CorePrototype;
        Generator generator = blueprint.GeneratorPrototype;
        GripModule grip = blueprint.GripModulePrototype;
        MoveModule move = blueprint.MoveModulePrototype;
        System system = blueprint.SystemPrototype;

        var tmp1 = new AssembledPiece([core, generator], "TMP1");

        return new List<IInstruction>
        {
            new ProduceInstruction(blueprint.Name),
            new GetOutStockInstruction(core),
            new GetOutStockInstruction(generator),
            new GetOutStockInstruction(grip),
            new GetOutStockInstruction(move),
            new InstallSystemInstruction(system, core),
            new AssembleInstruction("TMP1", core, generator),
            new AssembleInstruction(null, tmp1, grip),
            new AssembleInstruction(
                "TMP3",
                new AssembledPiece([tmp1, grip]),
                move
            ),
            new FinishInstruction(blueprint.Name),
        };
    }
    
    public void PrintInstructions(RobotBlueprint blueprint)
    {
        var instructions = GenerateInstructions(blueprint);
        foreach (var instruction in instructions)
        {
            Console.WriteLine(instruction);
        }
    }
}