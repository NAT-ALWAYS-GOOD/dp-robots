using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;
using DPRobots.Pieces;

namespace DPRobots;

public class Program
{
    public static void Main(string[] args)
    {
        var stock = new Dictionary<string, StockItem>
        {
            { "Core_CD1", new StockItem(new Core(CoreNames.Cd1, PieceCategory.Domestic), 5) },
            { "Core_CM1", new StockItem(new Core(CoreNames.Cm1, PieceCategory.Military), 5) },
            { "Core_CI1", new StockItem(new Core(CoreNames.Ci1, PieceCategory.Industrial), 5) },
            { "Generator_GD1", new StockItem(new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), 5) },
            { "Generator_GM1", new StockItem(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 5) },
            { "Generator_GI1", new StockItem(new Generator(GeneratorNames.Gi1, PieceCategory.Industrial), 5) },
            { "Arms_AM1", new StockItem(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 5) },
            { "Arms_AD1", new StockItem(new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), 5) },
            { "Arms_AI1", new StockItem(new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial), 5) },
            { "Legs_LM1", new StockItem(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 5) },
            { "Legs_LD1", new StockItem(new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic), 5) },
            { "Legs_LI1", new StockItem(new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial), 5) },
        };
        StockManager.GetInstance(stock);

        Console.WriteLine("Tapez votre commande ou 'EXIT' pour quitter.");

        while (true)
        {
            Console.Write("> ");

            var input = Console.ReadLine();

            // clear the console but keep the user input
            Console.Clear();
            Console.WriteLine($"> {input}");

            if (string.IsNullOrEmpty(input))
            {
                Logger.Log(LogType.ERROR, "Commande vide. Veuillez réessayer.");
                continue;
            }

            if (input.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Fin du programme.");
                break;
            }

            if (input.Trim().Equals("CLEAR", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                continue;
            }

            CommandHandler.HandleCommand(input);
        }
    }
}