namespace DPRobots.Pieces;

public enum CoreNames
{
    Cm1,
    Cd1,
    Ci1
}

public class Core(CoreNames name): Piece(name.ToString())
{
    private readonly CoreNames _name = name;
    
    private System? _system;
    
    public void InstallSystem(System system)
    {
        _system = system;
    }
    
    public override string ToString()
    {
        return $"Core_{_name.ToString().ToUpper()}";
    }
}