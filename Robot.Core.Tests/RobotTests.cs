using Xunit;

namespace Robot.Core.Tests
{
    public class RobotTests
    {
        private readonly Robot _robot = new Robot(5);

        [Fact]
        public void CanPlace()
        {
            _robot.Place(1, 2, Direction.East);
            Assert.Equal((true, 1, 2, Direction.East), _robot.Report());
        }
    }
}