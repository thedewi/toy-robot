using System;
using System.Linq;
using Xunit;

namespace Robot.Core.Tests
{
    public class RobotTests
    {
        private readonly Robot _robot = new Robot(5);

        [Fact]
        public void CanPlace()
        {
            Assert.Equal(PlacementValidity.Valid, _robot.Place(1, 2, Direction.East));
            Assert.Equal((true, 1, 2, Direction.East), _robot.Report());
        }

        [Fact]
        public void CanPlaceTwice()
        {
            CanPlace();
            Assert.Equal(PlacementValidity.Valid, _robot.Place(3, 4, Direction.South));
            Assert.Equal((true, 3, 4, Direction.South), _robot.Report());
        }

        [Fact]
        public void InvalidPlaceIsIgnored()
        {
            Assert.Equal(PlacementValidity.PosXOutOfRange, _robot.Place(5, 2, Direction.East));
            Assert.False(_robot.Report().onTable);
            Assert.Equal(PlacementValidity.PosYOutOfRange, _robot.Place(1, 5, Direction.East));
            Assert.False(_robot.Report().onTable);
            Assert.Equal(PlacementValidity.InvalidDirection,
                _robot.Place(1, 2, (Direction) (Enum.GetValues(typeof(Direction)).Cast<int>().Max() + 1)));
            Assert.False(_robot.Report().onTable);
            Assert.Equal(PlacementValidity.Valid, _robot.Place(1, 2, Direction.East));
            Assert.Equal((true, 1, 2, Direction.East), _robot.Report());
        }

        [Fact]
        public void CommandsIgnoredUntilPlace()
        {
            Assert.False(_robot.Report().onTable);
            Assert.Throws<NotOnTableException>(() => _robot.Move());
            Assert.False(_robot.Report().onTable);
            Assert.Equal(PlacementValidity.Valid, _robot.Place(1, 2, Direction.East));
            Assert.Equal((true, 1, 2, Direction.East), _robot.Report());
        }

        [Fact]
        public void CanMove()
        {
            Assert.Equal(PlacementValidity.Valid, _robot.Place(1, 2, Direction.East));
            Assert.True(_robot.Move());
            Assert.Equal((true, 2, 2, Direction.East), _robot.Report());
        }

        [Fact]
        public void AvoidsFallingOffTable()
        {
            Assert.Equal(PlacementValidity.Valid, _robot.Place(0, 0, Direction.West));
            Assert.False(_robot.Move());
            Assert.Equal((true, 0, 0, Direction.West), _robot.Report());
            Assert.Equal(PlacementValidity.Valid, _robot.Place(0, 0, Direction.South));
            Assert.False(_robot.Move());
            Assert.Equal((true, 0, 0, Direction.South), _robot.Report());
            Assert.Equal(PlacementValidity.Valid, _robot.Place(4, 4, Direction.North));
            Assert.False(_robot.Move());
            Assert.Equal((true, 4, 4, Direction.North), _robot.Report());
            Assert.Equal(PlacementValidity.Valid, _robot.Place(4, 4, Direction.East));
            Assert.False(_robot.Move());
            Assert.Equal((true, 4, 4, Direction.East), _robot.Report());
        }
    }
}