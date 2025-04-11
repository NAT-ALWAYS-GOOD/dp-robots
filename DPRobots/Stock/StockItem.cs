using DPRobots.Pieces;

namespace DPRobots.Stock;

public class StockItem(Piece prototype, int quantity)
{
    public Piece Prototype { get; } = prototype;
    public int Quantity { get; private set; } = quantity;
    
    public void DecreaseQuantity(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be non-negative.");
        
        if (amount > Quantity)
            throw new InvalidOperationException($"Not enough stock for {Prototype}");
        
        Quantity -= amount;
    }
}