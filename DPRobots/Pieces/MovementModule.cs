namespace DPRobots.Pieces;

public enum MoveModuleNames
{
    Lm1,
    Ld1,
    Li1
}

public class MoveModule(MoveModuleNames name): Piece(name.ToString())
{
    private readonly MoveModuleNames _name = name;
    
    public override string ToString()
    {
        return $"Legs_{_name.ToString().ToUpper()}";
    }
}