namespace DPRobots.Pieces;

public enum GeneratorNames
{
    Gm1,
    Gd1,
    Gi1
}

public class Generator(GeneratorNames name, PieceCategory category): Piece(name.ToString(), category)
{
    private readonly GeneratorNames _name = name;
    private readonly PieceCategory _category = category;

    public override object Clone()
    {
        return new Generator(_name, _category);
    }

    public override string ToString()
    {
        return $"Generator_{_name.ToString().ToUpper()}";
    }
}