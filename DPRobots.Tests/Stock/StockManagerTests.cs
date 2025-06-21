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
            Assert.Throws<InvalidOperationException>(() => StockManager.RemovePiece<Piece>(stockItem.Prototype));
        Assert.Equal($"Not enough stock for {piece}", exception.Message);
    }

    [Fact]
    public void Should_RemovePiece_When_StockIsAvailable()
    {
        var piece = (Core)PieceFactory.Create("Core_CD1");
        var stockItem = new StockItem(piece, 5);
        var stockManager = new StockManager();
        stockManager.Initialize([stockItem]);

        var removedPiece = StockManager.RemovePiece<Piece>(stockItem.Prototype);

        Assert.Equal(piece.ToString(), removedPiece.ToString());
        Assert.IsType<Core>(removedPiece);
        Assert.Equal(4, stockItem.Quantity);
    }

    [Fact]
    public void Should_Throw_InvalidOperationException_When_StockItemNotFound()
    {
        var item = new Generator(name: GeneratorNames.Gm1, category: PieceCategory.Military);
        var exception =
            Assert.Throws<InvalidOperationException>(() => StockManager.RemovePiece<Piece>(item));
        Assert.Equal($"No stock item found for piece: {item}", exception.Message);
    }

    [Fact]
    public void Should_AddRobot_When_RobotIsAdded()
    {
        StockManager.GetInstance().Initialize([
            new StockItem(PieceFactory.Create("Core_CD1"), 10),
            new StockItem(PieceFactory.Create("Generator_GD1"), 10),
            new StockItem(PieceFactory.Create("Arms_AD1"), 10),
            new StockItem(PieceFactory.Create("Legs_LD1"), 10)
        ]);
        var robot = Robot.FromName("RD-1");

        StockManager.AddRobot(robot);

        Assert.True(StockManager.GetInstance().GetStock.SingleOrDefault(item => item.Prototype.Equals(robot)) != null);
        Assert.Equal(1, StockManager.GetInstance().GetStock.Where(item => item.Prototype.Equals(robot)).First().Quantity);
    }

    [Fact]
    public void Should_IncreaseRobotQuantity_When_RobotAlreadyExists()
    {
        StockManager.GetInstance().Initialize([
            new StockItem(PieceFactory.Create("Core_CD1"), 10),
            new StockItem(PieceFactory.Create("Generator_GD1"), 10),
            new StockItem(PieceFactory.Create("Arms_AD1"), 10),
            new StockItem(PieceFactory.Create("Legs_LD1"), 10)
        ]);
        var robot = Robot.FromName("RD-1");

        StockManager.AddRobot(robot);
        StockManager.AddRobot(robot);

        Assert.NotNull(robot);
        Assert.True(StockManager.GetInstance().GetStock.SingleOrDefault(item => item.Prototype.Equals(robot)) != null);
        Assert.Equal(2, StockManager.GetInstance().GetStock.Where(item => item.Prototype.Equals(robot)).First().Quantity);
    }
}