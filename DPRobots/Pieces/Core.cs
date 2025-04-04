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
    
    public override string ToString()
    {
        return $"Core_{_name.ToString().ToUpper()}";
    }
}