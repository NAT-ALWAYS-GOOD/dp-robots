namespace DPRobots.UserInstructions;

public record HelpUserInstruction : IUserInstruction
{
    public const string CommandName = "HELP";

    public override string ToString() => CommandName;

    public static void Execute()
    {
        Console.WriteLine("Liste des commandes disponibles :");
        Console.WriteLine("HELP : Affiche cette aide.");
        Console.WriteLine("STOCKS : Affiche les stocks disponibles.");
        Console.WriteLine("NEEDED_STOCKS : Affiche les stocks requis pour construire les robots.");
        Console.WriteLine("INSTRUCTIONS : Affiche les instructions de construction des robots.");
        Console.WriteLine("VERIFY : Vérifie la commande.");
        Console.WriteLine("PRODUCE : Produit les robots.");
        Console.WriteLine("ADD_TEMPLATE : Ajoute un modèle de robot.");
    }
}