using DPRobots.Pieces;

namespace DPRobots;

public enum SystemNames
{
    Sb1,
}

public class System(SystemNames name, SystemCategory category)
{
    private readonly SystemNames _name = name;
    public readonly SystemCategory Category = category;
    
    public override string ToString()
    {
        return $"System_{_name.ToString().ToUpper()}";
    }
}