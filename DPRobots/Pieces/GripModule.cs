namespace DPRobots.Pieces;

public enum GripModuleNames
{
    Am1,
    Ad1,
    Ai1
}

public class GripModule(GripModuleNames name): Piece(name.ToString())
{
    private readonly GripModuleNames _name = name;
    
    public override string ToString()
    {
        return $"Arms_{_name.ToString().ToUpper()}";
    }
}