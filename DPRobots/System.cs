using DPRobots.Pieces;

namespace DPRobots;

public enum SystemNames
{
    Sb1,
}

public class System(SystemNames name, PieceCategory category)
{
    private readonly SystemNames _name = name;
    private readonly PieceCategory _category = category;
    
    public override string ToString()
    {
        return $"System_{_name.ToString().ToUpper()}";
    }
}