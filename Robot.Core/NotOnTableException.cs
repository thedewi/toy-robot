using System;

namespace Robot.Core
{
    public class NotOnTableException : Exception
    {
        public NotOnTableException() : base("Unable to proceed because the robot is not on the table.")
        {
        }
    }
}