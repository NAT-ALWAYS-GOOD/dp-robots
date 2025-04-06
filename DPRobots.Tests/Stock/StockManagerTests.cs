using DPRobots.Pieces;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests.Stock;

public class StockManagerTests
{
    [Fact]
    public void Should_RemovePiece_When_StockIsAvailable()
    {
        var piece = new Core(CoreNames.Cd1);
        var stockItem = new StockItem(piece, 5);
        var stockManager = new StockManager(new Dictionary<string, StockItem>
        {
            { stockItem.ToString(), stockItem }
        });

        var removedPiece = stockManager.RemovePiece<Piece>(stockItem.ToString());

        Assert.Equal(piece.ToString(), removedPiece.ToString());
        Assert.IsType<Core>(removedPiece);
        Assert.Equal(4, stockItem.Quantity);
    }
    
    [Fact]
    public void Should_Throw_InvalidOperationException_When_StockIsNotAvailable()
    {
        var piece = new Core(CoreNames.Cd1);
        var stockItem = new StockItem(piece, 0);
        var stockManager = new StockManager(new Dictionary<string, StockItem>
        {
            { stockItem.ToString(), stockItem }
        });

        var exception = Assert.Throws<InvalidOperationException>(() => stockManager.RemovePiece<Piece>(stockItem.ToString()));
        Assert.Equal($"Not enough stock for {piece.ToString()}", exception.Message);
    }
    
    [Fact]
    public void Should_Throw_InvalidOperationException_When_StockItemNotFound()
    {
        var stockManager = new StockManager(new Dictionary<string, StockItem>());
        var exception = Assert.Throws<InvalidOperationException>(() => stockManager.RemovePiece<Piece>("NonExistentKey"));
        Assert.Equal("No stock item found for key: NonExistentKey", exception.Message);
    }
}