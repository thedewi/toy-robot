using Xunit;

namespace Robot.Cli.Tests
{
    public class CliTests
    {
        private readonly CommandProcessor _processor = Program.CreateCommandProcessor();

        [Fact]
        public void CanPlace()
        {
            AssertCommandOutputs(
                ("PLACE 1,2,EAST", ""),
                ("REPORT", "1,2,EAST"));
        }

        [Fact]
        public void CommandsIgnoredUntilPlace()
        {
            AssertCommandOutputs(
                ("REPORT", ""),
                ("MOVE", ""),
                ("REPORT", ""),
                ("LEFT", ""),
                ("REPORT", ""),
                ("RIGHT", ""),
                ("REPORT", ""),
                ("PLACE 1,2,EAST", ""),
                ("REPORT", "1,2,EAST"));
        }

        [Fact]
        public void CanMove()
        {
            AssertCommandOutputs(
                ("PLACE 0,0,NORTH", ""),
                ("MOVE", ""),
                ("REPORT", "0,1,NORTH"));
        }

        [Fact]
        public void CanTurnLeft()
        {
            AssertCommandOutputs(
                ("PLACE 0,0,NORTH", ""),
                ("LEFT", ""),
                ("REPORT", "0,0,WEST"));
        }

        [Fact]
        public void CanTurnRight()
        {
            AssertCommandOutputs(
                ("PLACE 1,2,SOUTH", ""),
                ("RIGHT", ""),
                ("REPORT", "1,2,WEST"));
        }

        [Fact]
        public void InterpretsSequence()
        {
            AssertCommandOutputs(
                ("PLACE 1,2,EAST", ""),
                ("MOVE", ""),
                ("MOVE", ""),
                ("LEFT", ""),
                ("MOVE", ""),
                ("REPORT", "3,3,NORTH"));
        }

        private void AssertCommandOutputs(params (string command, string expectedOutput)[] assertions)
        {
            foreach (var (command, expectedOutput) in assertions)
            {
                string output;
                try
                {
                    output = _processor.Process(command);
                }
                catch (CommandException)
                {
                    output = "";
                }

                Assert.Equal(expectedOutput, output);
            }
        }
    }
}