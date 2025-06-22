namespace DPRobots.Stock;

public enum StockOperation
{
    Add,
    Remove
}

public class StockMovement(StockOperation operation, string itemName, int quantity, string? context = null)
{
    public DateTime Timestamp { get; } = DateTime.Now;
    public StockOperation Operation { get; } = operation;
    public string ItemName { get; } = itemName;
    public int Quantity { get; } = quantity;
    public string? Context { get; } = context;

    private static string OpSymbol(StockOperation op) => op switch
    {
        StockOperation.Add => "+",
        StockOperation.Remove => "-",
        _ => "?"
    };

    public override string ToString() =>
        $"{Timestamp:yyyy-MM-dd HH:mm:ss} => {OpSymbol(Operation)}{Quantity} {ItemName}" +
        (Context != null ? $" ({Context})" : "");
}

