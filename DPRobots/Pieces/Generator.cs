namespace DPRobots.Pieces;

public enum GeneratorNames
{
    Gm1,
    Gd1,
    Gi1
}

public class Generator(GeneratorNames name): Piece(name.ToString())
{
    private readonly GeneratorNames _name = name;
    
    public override string ToString()
    {
        return $"Generator_{_name.ToString().ToUpper()}";
    }
}