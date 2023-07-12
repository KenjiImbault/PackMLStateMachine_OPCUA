using System.Collections.Generic;
namespace PackML_v0
{
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
