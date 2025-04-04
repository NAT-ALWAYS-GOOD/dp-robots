namespace DPRobots;

public enum SystemNames
{
    Sb1,
}

public class System(SystemNames name)
{
    private readonly SystemNames _name = name;
    
    public override string ToString()
    {
        return $"System_{_name.ToString().ToUpper()}";
    }
}