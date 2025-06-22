using DPRobots.Logging;
using DPRobots.Pieces;
using DPRobots.RobotFactories;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests;

public class CommandHandlerTest
{
    private static RobotFactory _factory = new ("Usine");

    private static readonly CommandHandler CommandHandler;

    static CommandHandlerTest()
    {
        _factory.Templates.InitializeTemplates();
        List<StockItem> stock =
        [
            new(new Core(CoreNames.Cd1, PieceCategory.Domestic), 5),
            new(new Core(CoreNames.Cm1, PieceCategory.Military), 5),
            new(new Core(CoreNames.Ci1, PieceCategory.Industrial), 5),
            new(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 5),
            new(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 5),
            new(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 5)
        ];
        CommandHandler = new CommandHandler();
        _factory.Stock.Initialize(stock);
        FactoryManager.GetInstance().RegisterFactory(_factory);
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
        const string command = "VERIFY 2 XM-1, 1 RD-1 IN Usine";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Contains(LogType.UNAVAILABLE.ToString(), result);
    }

    [Fact]
    public void HandleCommand_Should_DisplayProduce_When_ProduceCommandProvided()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        _factory.Stock.Initialize([
            new StockItem(PieceFactory.Create("Core_CM1"), 10),
            new StockItem(PieceFactory.Create("Generator_GM1"), 10),
            new StockItem(PieceFactory.Create("Arms_AM1"), 10),
            new StockItem(PieceFactory.Create("Legs_LM1"), 10)
        ]);

        const string command = "PRODUCE 1 XM-1 IN Usine";

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
        Assert.Equal(1,
            _factory.Stock.GetStock.Where(item => item.Prototype.ToString() == "XM-1")
                .FirstOrDefault()?.Quantity);
    }

    [Fact]
    public void HandleCommand_Should_DisplayErrorMessage_When_CommandIsUnrecognized()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        const string command = "UNKNOWN_COMMAND";

        CommandHandler.HandleCommand(command);

        var result = output.ToString();
        Assert.Equal("ERROR Instruction `UNKNOWN_COMMAND` non reconnue.", result.Trim());
    }
}