using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Robot.Core;

namespace Robot.Cli
{
    public class CommandProcessor
    {
        public static readonly string[] CommandsAvailable = {"PLACE X,Y,F", "MOVE", "LEFT", "RIGHT", "REPORT"};

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
            switch (commandTokens[0].ToUpperInvariant())
            {
                case "PLACE":
                    PlaceCommand(commandTokens);
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

        private void PlaceCommand(IReadOnlyList<string> commandTokens)
        {
            if (commandTokens.Count == 4
                && int.TryParse(commandTokens[1], out var x)
                && int.TryParse(commandTokens[2], out var y)
                && Enum.TryParse<Direction>(commandTokens[3], true, out var direction)
                && _robot.Place(x, y, direction) == PlacementValidity.Valid)
                return;

            var maxDim = _robot.TableSideLength - 1;
            var dirOpts = string.Join("/",
                Enum.GetValues(typeof(Direction)).Cast<Direction>());
            throw new CommandException(
                "Unrecognized PLACE arguments."
                + Environment.NewLine
                + $"Usage:  PLACE X,Y,F  where X,Y are integers 0..{maxDim} and F is {dirOpts}.",
                false);
        }
    }
}