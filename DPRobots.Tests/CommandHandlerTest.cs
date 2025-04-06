using Xunit;

namespace DPRobots.Tests;

public class CommandHandlerTest
{
    private static readonly CommandHandler CommandHandler;

    static CommandHandlerTest()
    {
        CommandHandler = new CommandHandler();
    }

    [Fact]
    public void ParseCommand_Should_ReturnCorrectParts_When_ValidCommandProvided()
    {
        const string command = "INSTRUCTION 1 ROBOT";
        const string expectedInstruction = "INSTRUCTION";
        const string expectedArgs = "1 ROBOT";

        var (instruction, args) = CommandHandler.ParseCommand(command);

        Assert.Equal(expectedInstruction, instruction);
        Assert.Equal(expectedArgs, args);
    }

    [Fact]
    public void ParseCommand_Should_ReturnInstruction_When_NoArgsProvided()
    {
        const string command = "INSTRUCTION";
        const string expectedInstruction = "INSTRUCTION";

        var (instruction, args) = CommandHandler.ParseCommand(command);

        Assert.Equal(expectedInstruction, instruction);
        Assert.Equal(string.Empty, args);
    }

    [Fact]
    public void ParseCommand_Should_ThrowArgumentException_When_CommandIsEmpty()
    {
        const string command = "";

        var exception = Assert.Throws<ArgumentException>(() => CommandHandler.ParseCommand(command));

        Assert.Equal("La commande ne peut pas être vide", exception.Message);
    }

    [Fact]
    public void ParseArgs_Should_AggreateQuantity_When_SameRobotProvided()
    {
        const string args = "1 XM-1, 2 Rd-1, 3 XM-1";
        var expected = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "XM-1", 4 },
            { "RD-1", 2 }
        };

        var result = CommandHandler.ParseArgs(args);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseArgs_Should_ThrowArgumentException_When_NotEnoughTokens()
    {
        const string args = "1 XM-1, 2";

        var exception = Assert.Throws<ArgumentException>(() => CommandHandler.ParseArgs(args));

        Assert.Equal("Le format de l'argument '2' est incorrect. Attendu : 'quantité nom_de_robot'.",
            exception.Message);
    }

    [Fact]
    public void ParseArgs_Should_ThrowArgumentException_When_QuantityIsNotANumber()
    {
        const string args = "1 XM-1, two Rd-1";

        var exception = Assert.Throws<ArgumentException>(() => CommandHandler.ParseArgs(args));

        Assert.Equal("La quantité 'two' n'est pas un nombre valide.", exception.Message);
    }

    [Fact]
    public void ParseArgs_Should_ThrowArgumentException_When_TooManyTokens()
    {
        const string args = "1 XM-1, 2 Rd-1, 3 Extra Token";

        var exception = Assert.Throws<ArgumentException>(() => CommandHandler.ParseArgs(args));

        Assert.Equal("Le format de l'argument '3 Extra Token' est incorrect. Attendu : 'quantité nom_de_robot'.",
            exception.Message);
    }


    [Fact]
    public void HandleCommand_Should_DisplayStock_When_StocksCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        CommandHandler.HandleCommand("STOCKS");

        var result = output.ToString();
        Assert.Contains("Affichage des stocks disponibles :", result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayNeededStocks_When_NeededStocksCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "NEEDED_STOCKS 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("Affichage des stocks requis pour construire les robots :", result);
    }
    
    [Fact]
    public void HandleCommand_Should_DisplayInstructions_When_InstructionsCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "INSTRUCTIONS 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("Affichage des instructions de construction des robots :", result);
    }
    
    [Fact]
    public void HandleCommand_Should_Verify_When_VerifyCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "VERIFY 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("Affichage verification de la commande :", result);
    }
    
    [Fact]
    public void HandleCommand_Should_DisplayProduce_When_ProduceCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "PRODUCE 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("Affichage produce de la commande :", result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayErrorMessage_When_CommandIsUnrecognized()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "UNKNOWN_COMMAND";
        
        CommandHandler.HandleCommand(command);
        
        var result = output.ToString();
        Assert.Equal("ERROR Instruction non reconnue.", result.Trim());
    }
}