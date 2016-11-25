using System;
using System.Collections.Generic;

namespace Node.Net.Diagnostics
{
    public class ConsoleCommand : Command
    {
        public static ConsoleCommand Execute(string name,string directory = "",int timeout = 0)
        {
            var command = new ConsoleCommand { Name = name, Directory = directory, Timeout = timeout };
            command.Execute();
            return command;
        }

        public ConsoleCommand()
        {
            this[nameof(Type)] = nameof(ConsoleCommand);
        }

        public override ICommand Execute()
        {
            using (Executor executor = new Executor(this))
            {
                executor.Execute();
                return this;
            }
        }
    }
}
