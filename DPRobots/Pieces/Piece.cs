namespace DPRobots.Pieces;

public abstract class Piece(string name, PieceCategory? category) : ICloneable, IEquatable<Piece>
{
    private readonly string _name = name;
    public readonly PieceCategory? Category = category;

    public abstract object Clone();

    public override string ToString() => _name;

    public bool Equals(Piece? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _name == other._name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Piece)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), _name);
    }
}