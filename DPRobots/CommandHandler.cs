using DPRobots.Logging;
using DPRobots.UserInstructions;

namespace DPRobots;

public class CommandHandler
{
    private static readonly Dictionary<string, Func<string, IUserInstruction?>> RegisteredInstructions = new(StringComparer.OrdinalIgnoreCase)
    {
        [HelpUserInstruction.CommandName] = HelpUserInstruction.TryParse,
        [StocksUserInstruction.CommandName] = StocksUserInstruction.TryParse,
        [NeededStocksUserInstruction.CommandName] = NeededStocksUserInstruction.TryParse,
        [InstructionsUserInstruction.CommandName] = InstructionsUserInstruction.TryParse,
        [VerifyUserInstruction.CommandName] = VerifyUserInstruction.TryParse,
        [ProduceUserInstruction.CommandName] = ProduceUserInstruction.TryParse,
        [AddTemplateUserInstruction.CommandName] = AddTemplateUserInstruction.TryParse,
        [ReceiveUserInstruction.CommandName] = ReceiveUserInstruction.TryParse
    };

    
    /// <summary>
    /// Gère l'exécution d'une commande utilisateur
    /// Format attendu => INSTRUCTION ARGS
    /// ARGS est une liste de paires "quantité nom_de_robot" séparées par des virgules
    /// ARGS peut être vide
    ///
    /// Les instructions valides sont :
    /// HELP
    /// STOCKS
    /// NEEDED_STOCKS ARGS
    /// INSTRUCTIONS ARGS
    /// VERIFY ARGS
    /// PRODUCE ARGS
    /// ADD_TEMPLATE ARGS
    /// 
    /// </summary>
    /// <param name="commandLine"></param>
    public static void HandleCommand(string commandLine)
    {
        try
        {
            var (instructionName, args) = ParseCommand(commandLine);

            if (!RegisteredInstructions.TryGetValue(instructionName.ToUpper(), out var parser))
            {
                Logger.Log(LogType.ERROR, $"Instruction `{instructionName}` non reconnue.");
                return;
            }

            var instruction = parser(args);
            if (instruction is null)
            {
                Logger.Log(LogType.ERROR, $"Arguments invalides pour l'instruction `{instructionName}`.");
                return;
            }

            instruction.Execute();
        }
        catch (Exception ex)
        {
            Logger.Log(LogType.ERROR, ex.Message);
        }
    }
    
    public static (string Instruction, string Args) ParseCommand(string commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            throw new ArgumentException("La commande ne peut pas être vide");

        var firstSpaceIndex = commandLine.IndexOf(' ');

        if (firstSpaceIndex == -1)
        {
            return (commandLine.Trim(), string.Empty);
        }
        else
        {
            var instruction = commandLine.Substring(0, firstSpaceIndex).Trim();
            var args = commandLine.Substring(firstSpaceIndex + 1).Trim();
            return (instruction, args);
        }
    }
}