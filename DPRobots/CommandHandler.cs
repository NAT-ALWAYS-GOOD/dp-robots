using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.UserInstructions;

namespace DPRobots;

public class CommandHandler
{
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
    /// 
    /// </summary>
    /// <param name="commandLine"></param>
    public static void HandleCommand(string commandLine)
    {
        try
        {
            var (instruction, args) = ParseCommand(commandLine);

            switch (instruction.ToUpper())
            {
                case HelpUserInstruction.CommandName:
                    if (!IsArgsEmpty(args))
                    {
                        Logger.Log(LogType.ERROR, $"La commande {HelpUserInstruction.CommandName} ne prend pas d'arguments.");
                        return;
                    }

                    HelpUserInstruction.Execute();
                    break;

                case StocksUserInstruction.CommandName:
                    if (!IsArgsEmpty(args))
                    {
                        Logger.Log(LogType.ERROR, $"La commande {StocksUserInstruction.CommandName} ne prend pas d'arguments.");
                        return;
                    }

                    StocksUserInstruction.Execute();
                    break;

                case NeededStocksUserInstruction.CommandName:
                    var neededStocksArgs = ValidateAndParseArgs(args, NeededStocksUserInstruction.CommandName);
                    if (neededStocksArgs == null) return;
                    if (!VerifyRobots(neededStocksArgs)) return;

                    NeededStocksUserInstruction.Execute(neededStocksArgs);
                    break;

                case InstructionsUserInstruction.CommandName:
                    var instructionsArgs = ValidateAndParseArgs(args, InstructionsUserInstruction.CommandName);
                    if (instructionsArgs == null) return;
                    if (!VerifyRobots(instructionsArgs)) return;

                    InstructionsUserInstruction.Execute(instructionsArgs);
                    break;

                case VerifyUserInstruction.CommandName:
                    var verifyArgs = ValidateAndParseArgs(args, VerifyUserInstruction.CommandName);
                    if (verifyArgs == null) return;
                    if (!VerifyRobots(verifyArgs)) return;

                    VerifyUserInstruction.Execute(verifyArgs);
                    break;

                case ProduceUserInstruction.CommandName:
                    var produceArgs = ValidateAndParseArgs(args, ProduceUserInstruction.CommandName);
                    if (produceArgs == null) return;
                    if (!VerifyRobots(produceArgs)) return;

                    ProduceUserInstruction.Execute(produceArgs);
                    break;

                default:
                    Logger.Log(LogType.ERROR, "Instruction non reconnue.");
                    break;
            }
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
        }
    }

    private static bool IsArgsEmpty(string args)
    {
        return string.IsNullOrWhiteSpace(args);
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

    public static Dictionary<string, int> ParseArgs(string args)
    {
        var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var parts = args.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2)
                throw new ArgumentException(
                    $"Le format de l'argument '{trimmed}' est incorrect. Attendu : 'quantité nom_de_robot'.");

            if (!int.TryParse(tokens[0], out var quantity))
                throw new ArgumentException($"La quantité '{tokens[0]}' n'est pas un nombre valide.");

            var robotName = tokens[1].ToUpper();
            if (!result.TryAdd(robotName, quantity))
                result[robotName] += quantity;
        }

        return result;
    }

    /// <summary>
    /// Valide que la chaîne d'arguments n'est pas vide pour une commande donnée et la parse.
    /// Si les arguments sont vides, log un message d'erreur et retourne null.
    /// Sinon, retourne le dictionnaire obtenu via ParseArgs.
    /// </summary>
    private static Dictionary<string, int>? ValidateAndParseArgs(string args, string commandName)
    {
        if (!IsArgsEmpty(args))
            return ParseArgs(args);

        Logger.Log(LogType.ERROR, $"La commande {commandName} nécessite des arguments.");
        return null;
    }

    private static bool VerifyRobots(Dictionary<string, int> robotRequests)
    {
        var invalidRobot = robotRequests.Keys
            .FirstOrDefault(robotName => Robot.FromName(robotName) == null);

        if (invalidRobot == null) return true;

        Logger.Log(LogType.ERROR, $"`{invalidRobot}` is not a recognized robot");
        return false;
    }
}