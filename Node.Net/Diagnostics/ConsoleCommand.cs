using System;
using System.Collections.Generic;

namespace Node.Net.Diagnostics
{
    public class ConsoleCommand : Dictionary<string, dynamic>
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

        public void Execute()
        {
            using (Executor executor = new Executor(this))
            {
                executor.Execute();
            }
        }

        public string Name
        {
            get { return Get<string>(nameof(Name), string.Empty); }
            set { this[nameof(Name)] = value; }
        }

        public int ExitCode
        {
            get { return Get<int>(nameof(ExitCode), 0); }
            set { this[nameof(ExitCode)] = value; }
        }

        public int Timeout
        {
            get { return Get<int>(nameof(Timeout), 0); }
            set { this[nameof(Timeout)] = value; }
        }

        public string Output
        {
            get { return Get<string>(nameof(Output), string.Empty); }
            set { this[nameof(Output)] = value; }
        }

        public string Error
        {
            get { return Get<string>(nameof(Error), string.Empty); }
            set { this[nameof(Error)] = value; }
        }

        public string Directory
        {
            get { return Get<string>(nameof(Directory), string.Empty); }
            set { this[nameof(Directory)] = value; }
        }

        public string User
        {
            get { return Get<string>(nameof(User), string.Empty); }
            set { this[nameof(User)] = value; }
        }

        public string Machine
        {
            get { return Get<string>(nameof(Machine), string.Empty); }
            set { this[nameof(Machine)] = value; }
        }

        public DateTime StartTime
        {
            get { return Get<DateTime>(nameof(StartTime), DateTime.MinValue); }
            set { this[nameof(StartTime)] = value.ToString("o"); }
        }

        public DateTime EndTime
        {
            get { return Get<DateTime>(nameof(EndTime), DateTime.MinValue); }
            set { this[nameof(EndTime)] = value.ToString("o"); }
        }

        public string FileName
        {
            get
            {
                var filename = Get<string>(nameof(FileName), string.Empty);
                if (filename.Length == 0)
                {
                    filename = GetFileName(Name);
                }
                return filename;
            }
            set { this[nameof(FileName)] = value; }
        }

        public string Arguments
        {
            get
            {
                var name = Name.Trim();
                var parts = name.Split(' ');
                return name.Substring(parts[0].Length).Trim();
            }
        }

        private T Get<T>(string name, T defaultValue)
        {
            if (ContainsKey(name))
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return DateTime.Parse(this[name].ToString());
                }
                return (T)this[name];
            }
            return defaultValue;
        }

        public static string GetFileName(string command)
        {
            var target = command;

            var parts = command.Split(' ');
            if (parts.Length > 1) target = parts[0];

            var fname = target + ".exe";
            if (System.IO.File.Exists(fname)) return fname;

            fname = target + ".bat";
            if (System.IO.File.Exists(fname)) return fname;

            // search path
            var allpaths = System.Environment.GetEnvironmentVariable("PATH").Split(';');
            foreach (string path in allpaths)
            {
                fname = path + @"\" + target + ".bat";
                if (System.IO.File.Exists(fname)) { return fname; }

                fname = path + @"\" + target;
                if (System.IO.File.Exists(fname)) { return fname; }

                fname = path + @"\" + target + ".exe";
                if (System.IO.File.Exists(fname)) { return fname; }
            }


            return target;
        }
    }
}
