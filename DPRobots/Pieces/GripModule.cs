namespace DPRobots.Pieces;

public enum GripModuleNames
{
    Am1,
    Ad1,
    Ai1
}

public class GripModule(GripModuleNames name, PieceCategory category): Piece(name.ToString(), category)
{
    private readonly GripModuleNames _name = name;
    private readonly PieceCategory _category = category;
    
    public override object Clone()
    {
        return new GripModule(_name, _category);
    }
    
    public override string ToString()
    {
        return $"Arms_{_name.ToString().ToUpper()}";
    }
}