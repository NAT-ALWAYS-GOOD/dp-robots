using DPRobots.Instructions;
using DPRobots.Pieces;
using DPRobots.RobotFactories;

namespace DPRobots.Robots;

public class RobotBuilder(string name, RobotFactory factory)
{
    private RobotBlueprint? _blueprint;

    public RobotBuilder UseTemplate(RobotBlueprint? blueprint = null)
    {
        if (blueprint is not null)
        {
            _blueprint = blueprint;
            return this;
        }
        _blueprint = factory.Templates.Get(name);
        if (_blueprint is null)
            throw new InvalidOperationException($"`{name}` is not a recognized robot");
        return this;
    }

    public Robot Build(bool? printInstructions = true, string? context = null)
    {
        if (_blueprint is null)
            throw new InvalidOperationException("Template must be provided before building.");
        if (factory is null)
            throw new InvalidOperationException("Factory must be provided before building.");

        if (printInstructions == true)
        {
            InstructionsGenerator.GetInstance().PrintInstructions(_blueprint);
        }

        var robot = new Robot(name, blueprint: _blueprint);
        var robotComponents = factory.Stock.GetRobotComponents(robot.Blueprint, false, context);
        var core = robotComponents.Core;
        core.InstallSystem(_blueprint.SystemPrototype);
        robot.Core = core;
        robot.Generator = robotComponents.Generator;
        robot.GripModule = robotComponents.GripModule;
        robot.MoveModule = robotComponents.MoveModule;

        if (_blueprint.AdditionalModules is not null && _blueprint.AdditionalModules.Count > 0)
        {
            robot.AdditionalModules = new List<Piece>();

            foreach (var module in _blueprint.AdditionalModules)
            {
                var piece = factory.Stock.RemovePiece<Piece>(module, 1, context);
                robot.AdditionalModules.Add(piece);
            }
        }
        
        return robot;
    }
}