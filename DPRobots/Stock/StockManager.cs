using DPRobots.Pieces;

namespace DPRobots.Stock;

public class StockManager(Dictionary<string, StockItem> stock)
{
    private readonly Dictionary<string, StockItem> _stocks = stock;

    public T RemovePiece<T>(string key) where T : Piece
    {
        var stockItem = _stocks.GetValueOrDefault(key);
        if (stockItem == null)
            throw new InvalidOperationException($"No stock item found for key: {key}");

        stockItem.DecreaseQuantity(1);

        return (T)stockItem.Prototype.Clone();
    }
}