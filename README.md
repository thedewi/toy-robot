# Toy Robot Simulator

A solution to a [problem](PROBLEM.md).


# Command Line

To run the command line robot, make sure [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) is installed,
then clone the source and invoke `dotnet run`:

```cmd
git clone https://github.com/thedewi/toy-robot.git
dotnet run -p toy-robot\Robot.Cli
```


# Tests

To run tests, clone the source as above, and invoke `dotnet test`:

```cmd
dotnet test toy-robot
```


# Web visualization

A web app visualizing the same robot library is also included.

It is published online: https://davidstoyrobot.azurewebsites.net/


# Software structure overview

- `Robot.Core` is the .NET Standard library containing the core robot implementation, without user interface concerns.
- `Robot.Cli` is a .NET Core console application that runs the robot with console input, output, and diagnostic messages.
- `Robot.Web` is a Blazor server app that runs the robot with buttons and graphical output.
- `*.Tests` assemblies contain xunit tests.
