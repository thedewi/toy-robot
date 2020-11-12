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

        private void AssertCommandOutputs(params (string command, string expectedOutput)[] assertions)
        {
            foreach (var (command, expectedOutput) in assertions)
                Assert.Equal(expectedOutput, _processor.Process(command));
        }
    }
}