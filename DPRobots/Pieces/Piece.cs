namespace DPRobots.Pieces;

public abstract class Piece(string name)
{
    private readonly string _name = name;
    
    public override string ToString() => _name;
}