using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Robot.Cli.Tests")]

namespace Robot.Cli
{
    internal static class Program
    {
        private const int TableSideLength = 5;

        internal static CommandProcessor CreateCommandProcessor()
        {
            return new CommandProcessor(new Core.Robot(TableSideLength));
        }

        private static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        private static void WriteUsageInfo()
        {
            WriteInfo("Valid commands:  " +
                      string.Join("  |  ",
                          CommandProcessor.CommandsAvailable.Concat(new[] {"EXIT"})));
        }

        private static void Main()
        {
            WriteUsageInfo();

            var processor = CreateCommandProcessor();
            while (true)
            {
                Console.Write("> ");
                var commandLine = Console.ReadLine() ?? "";
                if (commandLine.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    break;

                try
                {
                    var output = processor.Process(commandLine);
                    if (!string.IsNullOrEmpty(output))
                        Console.WriteLine(output);
                }
                catch (CommandException ex)
                {
                    WriteInfo(ex.Message);
                    if (ex.DisplayUsage)
                        WriteUsageInfo();
                }
            }
        }
    }
}