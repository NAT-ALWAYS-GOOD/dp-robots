using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;
using DPRobots.Pieces;
using DPRobots.RobotFactories;

namespace DPRobots;

public class Program
{
    public static void Main(string[] args)
    {
        var robotTemplates = RobotTemplates.GetInstance();
        robotTemplates.InitializeTemplates();
        
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
        factoryManager.RegisterFactory(new RobotFactory("Usine1", stock));
        factoryManager.RegisterFactory(new RobotFactory("Usine2"));

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