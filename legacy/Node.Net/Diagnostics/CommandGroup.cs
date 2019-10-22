using System;
using System.Text;

namespace Node.Net.Diagnostics
{
    public class CommandGroup : Command
    {
        public CommandGroup()
        {
            this[nameof(Type)] = nameof(CommandGroup);
        }
        public override ICommand Execute()
        {
            StartTime = DateTime.Now;

            var output = new StringBuilder();
            var error = new StringBuilder();
            try
            {
                foreach (var key in Keys)
                {
                    var command = this[key] as ICommand;
                    if (command != null)
                    {
                        command.Execute();
                        output.Append(command.Summary);
                        if (command.ExitCode != 0)
                        {
                            ExitCode = command.ExitCode;
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                error.Append(exception.ToString());
            }
            finally
            {
                Output = output.ToString();
                Error = error.ToString();
                EndTime = DateTime.Now;
            }

            return this;
        }
    }
}
