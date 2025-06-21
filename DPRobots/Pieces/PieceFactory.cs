using DPRobots.Logging;
using DPRobots.UserInstructions;

namespace DPRobots.Pieces;

public static class PieceFactory
{
    private static readonly Dictionary<string, Func<Piece>> DefaultPieces = new()
    {
        { "Core_CM1", () => new Core(CoreNames.Cm1, PieceCategory.Military) },
        { "Core_CD1", () => new Core(CoreNames.Cd1, PieceCategory.Domestic) },
        { "Core_CI1", () => new Core(CoreNames.Ci1, PieceCategory.Industrial) },

        { "Generator_GM1", () => new Generator(GeneratorNames.Gm1, PieceCategory.Military) },
        { "Generator_GD1", () => new Generator(GeneratorNames.Gd1, PieceCategory.Domestic) },
        { "Generator_GI1", () => new Generator(GeneratorNames.Gi1, PieceCategory.Industrial) },

        { "Arms_AM1", () => new GripModule(GripModuleNames.Am1, PieceCategory.Military) },
        { "Arms_AD1", () => new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic) },
        { "Arms_AI1", () => new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial) },

        { "Legs_LM1", () => new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military) },
        { "Legs_LD1", () => new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic) },
        { "Legs_LI1", () => new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial) },

        { "System_SB1", () => new System(SystemNames.Sb1, PieceCategory.General) },
        { "System_SM1", () => new System(SystemNames.Sm1, PieceCategory.Military) },
        { "System_SD1", () => new System(SystemNames.Sd1, PieceCategory.Domestic) },
        { "System_SI1", () => new System(SystemNames.Si1, PieceCategory.Industrial) }
    };

    public static Piece Create(string name)
    {
        if (IsAssemblyName(name))
        {
            var partNames = UserInstructionArgumentParser.ParseAssemblyName(name);
            var parts = partNames.Select(Create).ToList();
            return new AssembledPiece(parts);
        }

        if (DefaultPieces.TryGetValue(name, out var piece))
            return piece();

        Logger.Log(LogType.ERROR, $"No piece found with name '{name}'");
        throw new ArgumentException($"No piece named '{name}'");
    }

    public static Piece? TryCreate(string name)
    {
        if (IsAssemblyName(name))
        {
            try
            {
                var partNames = UserInstructionArgumentParser.ParseAssemblyName(name);
                var parts = partNames.Select(TryCreate).ToList();
                if (parts.Any(p => p == null)) return null;
                return new AssembledPiece(parts!);
            }
            catch
            {
                return null;
            }
        }

        return DefaultPieces.TryGetValue(name, out var piece) ? piece() : null;
    }


    private static bool IsAssemblyName(string name) =>
        name.StartsWith("[") && name.EndsWith("]");
}