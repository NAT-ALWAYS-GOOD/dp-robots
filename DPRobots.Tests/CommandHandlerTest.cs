using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests;

public class CommandHandlerTest
{
    private static readonly CommandHandler CommandHandler;

    static CommandHandlerTest()
    {
        Dictionary<string, StockItem> Stock = new Dictionary<string, StockItem>
        {
            { "Core_CD1", new StockItem(new Core(CoreNames.Cd1), 5) },
            { "Core_CM1", new StockItem(new Core(CoreNames.Cm1), 5) },
            { "Core_CI1", new StockItem(new Core(CoreNames.Ci1), 5) },
            { "Generator_GM1", new StockItem(new Generator(GeneratorNames.Gm1), 5) },
            { "Arms_AM1", new StockItem(new GripModule(GripModuleNames.Am1), 5) },
            { "Legs_LM1", new StockItem(new MoveModule(MoveModuleNames.Lm1), 5) },
        };
        CommandHandler = new CommandHandler();
        StockManager.Initialize(Stock);
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
        Assert.Contains("Core_CD1", result);
        Assert.Contains("Core_CM1", result);
        Assert.Contains("Core_CI1", result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayNeededStocks_When_NeededStocksCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "NEEDED_STOCKS 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("2 Core_CM1", result);
        Assert.Contains("2 Generator_GM1", result);
        Assert.Contains("2 Arms_AM1", result);
        Assert.Contains("2 Legs_LM1", result);
        Assert.Contains("1 Core_CD1", result);
        Assert.Contains("1 Generator_GD1", result);
        Assert.Contains("1 Arms_AD1", result);
        Assert.Contains("1 Legs_LD1", result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayInstructions_When_InstructionsCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "INSTRUCTIONS 1 XM-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();

        Assert.Contains("PRODUCING XM-1", result);
        Assert.Contains("GET_OUT_STOCK 1 Core_CM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Generator_GM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Arms_AM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Legs_LM1", result);
        Assert.Contains("INSTALL System_SB1 Core_CM1", result);
        Assert.Contains("ASSEMBLE TMP1 Core_CM1 Generator_GM1", result);
        Assert.Contains("ASSEMBLE TMP1 Arms_AM1", result);
        Assert.Contains("ASSEMBLE TMP3 [TMP1,Arms_AM1] Legs_LM1", result);
        Assert.Contains("FINISHED XM-1", result);
    }

    [Fact]
    public void HandleCommand_Should_Verify_When_VerifyCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "VERIFY 2 XM-1, 1 RD-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains(LogType.UNAVAILABLE.ToString(), result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayProduce_When_ProduceCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "PRODUCE 1 XM-1";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains("PRODUCING XM-1", result);
        Assert.Contains("GET_OUT_STOCK 1 Core_CM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Generator_GM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Arms_AM1", result);
        Assert.Contains("GET_OUT_STOCK 1 Legs_LM1", result);
        Assert.Contains("INSTALL System_SB1 Core_CM1", result);
        Assert.Contains("ASSEMBLE TMP1 Core_CM1 Generator_GM1", result);
        Assert.Contains("ASSEMBLE TMP1 Arms_AM1", result);
        Assert.Contains("ASSEMBLE TMP3 [TMP1,Arms_AM1] Legs_LM1", result);
        Assert.Contains("FINISHED XM-1", result);

        // verify stock
        Assert.Equal(1, StockManager.Instance!.GetRobotStocks["XM-1"].Quantity);
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