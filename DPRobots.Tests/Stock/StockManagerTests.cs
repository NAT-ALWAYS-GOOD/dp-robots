using DPRobots.Pieces;
using DPRobots.Robots;
using DPRobots.Stock;
using Xunit;

namespace DPRobots.Tests.Stock;

public class StockManagerTests
{
    [Fact]
    public void Should_Throw_InvalidOperationException_When_StockIsNotAvailable()
    {
        var piece = (Core)PieceFactory.Create("Core_CD1");
        var stockItem = new StockItem(piece, 0);
        var stockManager = new StockManager();
        stockManager.Initialize([stockItem]);

        var exception =
            Assert.Throws<InvalidOperationException>(() => stockManager.RemovePiece<Piece>(stockItem.Prototype));
        Assert.Equal($"Not enough stock for {piece}", exception.Message);
    }

    [Fact]
    public void Should_RemovePiece_When_StockIsAvailable()
    {
        var piece = (Core)PieceFactory.Create("Core_CD1");
        var stockItem = new StockItem(piece, 5);
        var stockManager = new StockManager();
        stockManager.Initialize([stockItem]);

        var removedPiece = stockManager.RemovePiece<Piece>(stockItem.Prototype);

        Assert.Equal(piece.ToString(), removedPiece.ToString());
        Assert.IsType<Core>(removedPiece);
        Assert.Equal(4, stockItem.Quantity);
    }

    [Fact]
    public void Should_Throw_InvalidOperationException_When_StockItemNotFound()
    {
        var stockManager = StockManager.GetInstance();
        var item = new Generator(name: GeneratorNames.Gm1, category: PieceCategory.Military);
        var exception =
            Assert.Throws<InvalidOperationException>(() => stockManager.RemovePiece<Piece>(item));
        Assert.Equal($"No stock item found for piece: {item}", exception.Message);
    }

    [Fact]
    public void Should_AddRobot_When_RobotIsAdded()
    {
        var robot = new Rd1();
        var stockManager = StockManager.GetInstance();

        stockManager.AddRobot(robot);

        Assert.True(stockManager.GetRobotStocks.SingleOrDefault(item => item.RobotPrototype == robot) != null);
        Assert.Equal(1, stockManager.GetRobotStocks.Where(item => item.RobotPrototype == robot).First().Quantity);
    }

    [Fact]
    public void Should_IncreaseRobotQuantity_When_RobotAlreadyExists()
    {
        var robot = new Rd1();
        var stockManager = StockManager.GetInstance();

        stockManager.AddRobot(robot);
        stockManager.AddRobot(robot);

        Assert.True(stockManager.GetRobotStocks.SingleOrDefault(item => item.RobotPrototype == robot) != null);
        Assert.Equal(2, stockManager.GetRobotStocks.Where(item => item.RobotPrototype == robot).First().Quantity);
    }
}