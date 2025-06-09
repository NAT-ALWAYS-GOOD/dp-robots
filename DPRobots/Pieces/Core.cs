namespace DPRobots.Pieces;

public enum CoreNames
{
    Cm1,
    Cd1,
    Ci1
}

public class Core(CoreNames name, PieceCategory category): Piece(name.ToString(), category)
{
    private readonly CoreNames _name = name;
    private readonly PieceCategory _category = category;
    
    private System? _system;
    
    public void InstallSystem(System system)
    {
        _system = system;
    }

    public override object Clone()
    {
        return new Core(_name, _category);
    }

    public override string ToString()
    {
        return $"Core_{_name.ToString().ToUpper()}";
    }
}