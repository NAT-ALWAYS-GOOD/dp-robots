namespace DPRobots.Pieces;

public abstract class Piece(string name) : ICloneable
{
    private readonly string _name = name;
    
    public abstract object Clone();
    
    public override string ToString() => _name;
}