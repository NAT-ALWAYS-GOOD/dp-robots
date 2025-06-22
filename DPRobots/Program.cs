using DPRobots.Logging;
using DPRobots.Stock;
using DPRobots.Pieces;
using DPRobots.RobotFactories;

namespace DPRobots;

public class Program
{
    public static void Main(string[] args)
    {
        List<StockItem> stock =
        [
            new(new Core(CoreNames.Cd1, PieceCategory.Domestic), 5),
            new(new Core(CoreNames.Cm1, PieceCategory.Military), 5),
            new(new Core(CoreNames.Ci1, PieceCategory.Industrial), 5),
            new(new Generator(GeneratorNames.Gd1, PieceCategory.Domestic), 5),
            new(new Generator(GeneratorNames.Gm1, PieceCategory.Military), 5),
            new(new Generator(GeneratorNames.Gi1, PieceCategory.Industrial), 5),
            new(new GripModule(GripModuleNames.Am1, PieceCategory.Military), 5),
            new(new GripModule(GripModuleNames.Ad1, PieceCategory.Domestic), 5),
            new(new GripModule(GripModuleNames.Ai1, PieceCategory.Industrial), 5),
            new(new MoveModule(MoveModuleNames.Lm1, PieceCategory.Military), 5),
            new(new MoveModule(MoveModuleNames.Ld1, PieceCategory.Domestic), 5),
            new(new MoveModule(MoveModuleNames.Li1, PieceCategory.Industrial), 5)
        ];
        
        var factoryManager = FactoryManager.GetInstance();
        var factory1 = new RobotFactory("Usine1", stock);
        factory1.Templates.InitializeTemplates();
        factoryManager.RegisterFactory(factory1);
        var factory2 = new RobotFactory("Usine2");
        factory2.Templates.InitializeTemplates();
        factoryManager.RegisterFactory(factory2);

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