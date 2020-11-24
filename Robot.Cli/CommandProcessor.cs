using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Robot.Core;

namespace Robot.Cli
{
    public class CommandProcessor
    {
        public static readonly string[] CommandsAvailable =
            {"PLACE X,Y,F", "MOVE", "LEFT", "RIGHT", "REPORT", "BLOCK X,Y"};

        private static readonly Regex SplitPattern = new Regex(@"[\s,]+", RegexOptions.Compiled
                                                                          | RegexOptions.CultureInvariant);

        private readonly Core.Robot _robot;

        public CommandProcessor(Core.Robot robot)
        {
            _robot = robot;
        }

        public string Process(string commandLine)
        {
            var commandTokens = SplitPattern.Split(commandLine);
            try
            {
                switch (commandTokens[0].ToUpperInvariant())
                {
                    case "PLACE":
                        PlaceCommand(commandTokens);
                        return "";
                    case "MOVE":
                        MoveCommand();
                        return "";
                    case "LEFT":
                        _robot.Left();
                        return "";
                    case "RIGHT":
                        _robot.Right();
                        return "";
                    case "BLOCK":
                        BlockCommand(commandTokens);
                        return "";
                    case "REPORT":
                        var (onTable, posX, posY, direction) = _robot.Report();
                        return onTable
                            ? $"{posX},{posY},{direction.ToString().ToUpperInvariant()}"
                            : "";
                    default:
                        throw new CommandException("Unrecognized command.", true);
                }
            }
            catch (NotOnTableException ex)
            {
                throw new CommandException(ex.Message, false);
            }
        }

        private void PlaceCommand(IReadOnlyList<string> commandTokens)
        {
            if (commandTokens.Count == 4
                && int.TryParse(commandTokens[1], out var x)
                && int.TryParse(commandTokens[2], out var y)
                && Enum.TryParse<Direction>(commandTokens[3], true, out var direction))
            {
                var placementValidity = _robot.Place(x, y, direction);
                switch (placementValidity)
                {
                    case PlacementValidity.PosXOutOfRange:
                    case PlacementValidity.PosYOutOfRange:
                        throw new CommandException(
                            "Unable to place robot because the position is not on the table.", false);
                    case PlacementValidity.InvalidDirection:
                        throw new CommandException(
                            $"Unable to place robot in unsupported direction '{direction}'.", false);
                    case PlacementValidity.Blocked:
                        throw new CommandException("Unable to place robot because the position is blocked.",
                            false);
                    case PlacementValidity.Valid:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var maxDim = _robot.TableSideLength - 1;
            var dirOpts = string.Join("/",
                Enum.GetValues(typeof(Direction)).Cast<Direction>());
            throw new CommandException(
                "Unrecognized PLACE arguments."
                + Environment.NewLine
                + $"Usage:  PLACE X,Y,F  where X,Y are integers 0..{maxDim} and F is {dirOpts}.",
                false);
        }

        private void MoveCommand()
        {
            var moveValidity = _robot.Move();
            switch (moveValidity)
            {
                case PlacementValidity.Valid:
                    return;
                case PlacementValidity.PosXOutOfRange:
                case PlacementValidity.PosYOutOfRange:
                    throw new CommandException("Unable to move due to table edge.",
                        false);
                case PlacementValidity.Blocked:
                    throw new CommandException("Unable to move due to blocked square.",
                        false);
                case PlacementValidity.InvalidDirection:
                    throw new CommandException("Unable to move due to invalid direction.",
                        false);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BlockCommand(string[] commandTokens)
        {
            if (commandTokens.Length == 3
                && int.TryParse(commandTokens[1], out var x)
                && int.TryParse(commandTokens[2], out var y))
            {
                if (_robot.Block(x, y) != PlacementValidity.Valid)
                    throw new CommandException("Unable to block position because it is not on the table.",
                        false);
                return;
            }

            var maxDim = _robot.TableSideLength - 1;
            throw new CommandException(
                "Unrecognized BLOCK arguments."
                + Environment.NewLine
                + $"Usage:  BLOCK X,Y  where X,Y are integers 0..{maxDim}.",
                false);
        }
    }
}