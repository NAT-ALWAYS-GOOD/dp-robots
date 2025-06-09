using DPRobots.Pieces;

namespace DPRobots;

public enum SystemNames
{
    Sb1,
    Sm1,
    Sd1,
    Si1,
}

public class System(SystemNames name, PieceCategory category) : Piece(name.ToString(), category)
{
    private readonly SystemNames _name = name;
    private readonly PieceCategory _category = category;

    public override object Clone()
    {
        return new System(_name, _category);
    }
    
    public override string ToString()
    {
        return $"System_{_name.ToString().ToUpper()}";
    }
}