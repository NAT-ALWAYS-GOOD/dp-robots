namespace DPRobots.Pieces;

public class AssembledPiece : Piece
{
    public List<Piece> Pieces { get; }
    
    public AssembledPiece(List<Piece> pieces) : base(ComputeName(pieces))
    {
        VerifyPieces(pieces);
        Pieces = Flatten(pieces);
    }

    public AssembledPiece(List<Piece> pieces, string name) : base(name)
    {
        VerifyPieces(pieces);
        Pieces = Flatten(pieces);
    }

    private static void VerifyPieces(List<Piece> pieces)
    {
        if (pieces == null || pieces.Count < 1) throw new ArgumentException("Pieces cannot be null or empty");
    }

    private static List<Piece> Flatten(List<Piece> pieces)
    {
        List<Piece> flattenedPieces = [];
        foreach (var piece in pieces)
            if (piece is AssembledPiece assembledPiece)
                flattenedPieces.AddRange(assembledPiece.Pieces);
            else
                flattenedPieces.Add(piece);

        return flattenedPieces;
    }

    private static string ComputeName(List<Piece> pieces)
    {
        return "[" + string.Join(",", Flatten(pieces).Select(p => p.ToString())) + "]";
    }
}