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
        List<Piece> additionalModules = blueprint.AdditionalModules ?? new List<Piece>();

        var tmp1 = new AssembledPiece([core, generator], "TMP1");
        var tmp2 = new AssembledPiece([tmp1, grip]);
        var tmp3 = new AssembledPiece([tmp2, move], "TMP3");

        var instructions = new List<IInstruction>
        {
            new ProduceInstruction(blueprint.Name),
            new GetOutStockInstruction(core),
            new GetOutStockInstruction(generator),
            new GetOutStockInstruction(grip),
            new GetOutStockInstruction(move),
            new InstallSystemInstruction(system, core),
            new AssembleInstruction("TMP1", core, generator),
            new AssembleInstruction(null, tmp1, grip),
            new AssembleInstruction("TMP3", tmp2, move)
        };

        var currentBase = tmp3;
        int index = 4;
        foreach (var module in additionalModules)
        {
            instructions.Add(new GetOutStockInstruction(module));

            var nextTmp = new AssembledPiece([currentBase, module], $"TMP{index++}");
            instructions.Add(new AssembleInstruction(nextTmp.ToString(), currentBase, module));
            currentBase = nextTmp;
        }
        instructions.Add(new FinishInstruction(blueprint.Name));
        
        return instructions;
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