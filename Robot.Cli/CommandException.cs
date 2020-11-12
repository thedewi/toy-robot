using System;

namespace Robot.Cli
{
    public class CommandException : Exception
    {
        public CommandException(string message, bool displayUsage) : base(message)
        {
            DisplayUsage = displayUsage;
        }

        public bool DisplayUsage { get; }
    }
}