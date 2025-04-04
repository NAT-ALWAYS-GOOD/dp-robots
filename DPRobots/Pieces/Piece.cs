namespace DPRobots.Pieces;

public abstract class Piece(string name)
{
    private string Name { get; } = name;
    
    public override string ToString() => Name;
}