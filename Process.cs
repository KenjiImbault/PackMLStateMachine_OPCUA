using System.Collections.Generic;
namespace PackML_v0
{
    /// <summary>
    /// Class <c>Process</c> models a process, which is a series of command to apply to a machine. (For example: Start -> Complete -> Reset)
    /// It contains the list of commands to apply
    /// The time needed to execute the StateComplete command (called SCtime)
    /// The time needed to execute all the other commands (called CommandTime)
    /// </summary>
    public class Process
    {
        private readonly List<Command> Commands;
        private readonly int SCTime;
        private readonly int CommandTime;
        public Process(List<Command> commands, int scTime, int commandTime)
        {
            Commands = commands;
            SCTime = scTime;
            CommandTime = commandTime;
        }

        public List<Command> GetCommands()
        {
            return Commands;
        }

        public int GetSCTime()
        {
            return SCTime;
        }

        public int GetCommandTime()
        {
            return CommandTime;
        }

    }
}
