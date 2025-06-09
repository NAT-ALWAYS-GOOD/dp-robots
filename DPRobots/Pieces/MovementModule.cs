namespace DPRobots.Pieces;

public enum MoveModuleNames
{
    Lm1,
    Ld1,
    Li1
}

public class MoveModule(MoveModuleNames name, PieceCategory category) : Piece(name.ToString(), category)
{
    private readonly MoveModuleNames _name = name;
    private readonly PieceCategory _category = category;

    public override object Clone()
    {
        return new MoveModule(_name, _category);
    }

    public override string ToString()
    {
        return $"Legs_{_name.ToString().ToUpper()}";
    }
}