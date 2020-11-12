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
                ("PLACE 1,2,EAST", ""),
                ("REPORT", "1,2,EAST"));
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