using DPRobots.Robots;

namespace DPRobots.Stock;

public class RobotStockItem(Robot robotPrototype, int quantity)
{
    public Robot RobotPrototype { get; } = robotPrototype;
    public int Quantity { get; private set; } = quantity;

    public void IncreaseQuantity(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be non-negative.");

        Quantity += amount;
    }
}