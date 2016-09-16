namespace Node.Net.Deprecated.Environment
{
    [System.Serializable]
    #if NET35
    public class Command : System.Collections.Generic.Dictionary<string,object>, System.IComparable
    #else
    public class Command : System.Collections.Generic.Dictionary<string,dynamic>, System.IComparable
    #endif
    {
        public static Command Execute(string cmd, int timeout)
        {
            var command = new Command(cmd, timeout);
            command.Execute();
            return command;
        }
        public static Command Execute(string cmd,string workingDirectory,int timeout)
        {
            var command = new Command(cmd, workingDirectory, timeout);
            command.Execute();
            return command;
        }
        public Command() { StartTime = System.DateTime.Now; this["Type"] = nameof(Command); }
        public Command(string cmd) { StartTime = System.DateTime.Now; this["Type"] = nameof(Command); this[nameof(Name)] = cmd; }
        public Command(string cmd, string workingDirectory) { StartTime = System.DateTime.Now; this["Type"] = nameof(Command); this[nameof(Name)] = cmd; Directory = workingDirectory; }
        public Command(string cmd, string workingDirectory, int timeout) { StartTime = System.DateTime.Now; this["Type"] = nameof(Command); this[nameof(Name)] = cmd; Directory = workingDirectory; Timeout = timeout; }
        public Command(string cmd, int timeout) { StartTime = System.DateTime.Now; this["Type"] = nameof(Command); this[nameof(Name)] = cmd; Timeout = timeout; }

        private Command(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context) : base(info,context)
        {
        }

        public string Name
        {
            get { return getString(nameof(Name));}
            private set { this[nameof(Name)] = value; }
        }

        private string getString(string key) => ContainsKey(key) ? this[key].ToString() : "";

        public string FileName
        {
            get 
            {
                if(!ContainsKey(nameof(FileName)))
                {
                    this[nameof(FileName)] = GetFileName(Name);
                }
                return getString(nameof(FileName));
            }
        }

        public string Directory
        {
            get
            {
                return getString(nameof(Directory));
            }
            set
            {
                this[nameof(Directory)] = value;
            }
        }

        public int Timeout
        {
            get
            {
                if (ContainsKey(nameof(Timeout))) return (int)(double)this[nameof(Timeout)];
                return 0;
            }
            set
            {
                this[nameof(Timeout)] = (double)value;
            }
        }

        public int Execute()
        {
            using(Internal.Executor executor = new Internal.Executor(this))
            {
                executor.Execute();
            }
            // TODO: log here
            return ExitCode;
        }
        
        public bool HasExecuted
        {
            get
            {
                if (!ContainsKey(nameof(EndTime))) return false;
                return true;
            }
        }
        public int ExitCode
        {
            get
            {
                if (ContainsKey(nameof(ExitCode))) return (int)(double)this[nameof(ExitCode)];
                return 0;
            }
            set
            {
                this[nameof(ExitCode)] = (double)value;
            }
        }

        public string User
        {
            get
            {
                if (ContainsKey(nameof(User))) return this[nameof(User)].ToString();
                return System.Environment.UserName;
            }
            set { this[nameof(User)] = value; }
        }

        public string Machine
        {
            get
            {
                if (ContainsKey(nameof(Machine))) return this[nameof(Machine)].ToString();
                return System.Environment.MachineName;
            }
            set { this[nameof(Machine)] = value; }
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

        public System.DateTime StartTime
        {
            get
            {
                if (ContainsKey(nameof(StartTime)))
                {
                    return System.DateTime.Parse(this[nameof(StartTime)].ToString());
                }
                this[nameof(StartTime)] = System.DateTime.Now.ToString("o");
                return System.DateTime.Parse(this[nameof(StartTime)].ToString());
            }
            set
            {
                this[nameof(StartTime)] = value.ToString("o");
            }
        }

        public System.DateTime EndTime
        {
            get
            {
                if(ContainsKey(nameof(EndTime)))
                {
                    return System.DateTime.Parse(this[nameof(EndTime)].ToString());
                }
                this[nameof(EndTime)] = System.DateTime.Now.ToString("o");
                return System.DateTime.Parse(this[nameof(EndTime)].ToString());
            }
            set
            {
                this[nameof(EndTime)] = value.ToString("o");
            }
        }

        public System.TimeSpan Duration => EndTime - StartTime;

        public string Output
        {
            get
            {
                if (ContainsKey(nameof(Output))) return this[nameof(Output)].ToString();
                return "";
            }
            set
            {
                this[nameof(Output)] = value;
            }
        }

        public void AppendOutput(string value)
        {
            Output = Output.Length == 0 ? value + System.Environment.NewLine : Output + value + System.Environment.NewLine;
        }

        public string Error
        {
            get
            {
                if (ContainsKey(nameof(Error))) return this[nameof(Error)].ToString();
                return "";
            }
            set
            {
                this[nameof(Error)] = value;
            }
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

        public void Merge(Command other)
        {
            if (ReferenceEquals(null, other)) return;
            var durationStr = "";
            var status = " not executed";

            if (true)//other.Duration.HasValue)
            {
                durationStr = " [" + GetFormattedTimeSpan(other.Duration) + "]";
                status = other.ExitCode == 0 ? " OK" : " Error";
            }

            if (other.ExitCode != 0)
            {
                ExitCode = other.ExitCode;
                Error = Error.Length == 0 ? other.Name + durationStr + status : Error + System.Environment.NewLine + durationStr + status;
            }
            if(other.Error.Length > 0)
            {
                Error = Error.Length == 0 ? other.Error : Error + System.Environment.NewLine + other.Error;
            }

            Output = Output.Length == 0 ? other.Name + durationStr + status : Output + System.Environment.NewLine + other.Name + durationStr + status;
            if (other.Output.Length > 0)
            {
                Output = Output.Length == 0 ? other.Output : Output + System.Environment.NewLine + other.Output;
            }

            foreach(string key in other.Keys)
            {
                if(!ContainsKey(key))
                {
                    this[key] = other[key];
                }
            }
        }

        public override int GetHashCode() => GetHashCode(this);

        public static int GetHashCode(object value)
        {
            if (!ReferenceEquals(null, value))
            {
                if (value.GetType() == typeof(bool) ||
                   value.GetType() == typeof(double) ||
                    value.GetType() == typeof(string)) return value.GetHashCode();
                else
                {
                    if (typeof(System.Collections.IDictionary).IsAssignableFrom(value.GetType())) return GetHashCode(value as System.Collections.IDictionary);
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType())) return GetHashCode(value as System.Collections.IEnumerable);
                }
            }
            return 0;
        }
        public static int GetHashCode(System.Collections.IEnumerable value)
        {
            var count = 0;
            var hashCode = 0;
            foreach (object item in value)
            {
                var tmp = GetHashCode(item);
                if (tmp != 0) count++;
                hashCode = hashCode ^ tmp;
            }
            hashCode = hashCode ^ count;
            return hashCode;
        }
        public static int GetHashCode(System.Collections.IDictionary value)
        {
            var hashCode = value.Count;
            foreach (string key in value.Keys)
            {
                hashCode = hashCode ^ GetHashCode(key) ^ GetHashCode(value[key]);
            }
            return hashCode;
        }
        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }

        public int CompareTo(object value)
        {
            if (ReferenceEquals(this, value)) return 0;
            if (ReferenceEquals(null, value)) return 1;

            var thatCommand = value as Command;
            if (!ReferenceEquals(null, thatCommand))
            {
                return thatCommand.StartTime.CompareTo(StartTime);
            }
            return GetHashCode().CompareTo(GetHashCode(value));
        }

        public static string GetFormattedTimeSpan(System.TimeSpan duration)
        {
            if (duration.TotalSeconds < 1)
            {
                return System.Math.Round(duration.TotalMilliseconds).ToString() + " ms";
            }
            if (duration.TotalMinutes < 1)
            {
                return System.Math.Round(duration.TotalSeconds).ToString() + " sec";
            }
            if (duration.TotalHours < 1)
            {
                if(duration.TotalMinutes >= 10) return System.Math.Round(duration.TotalMinutes).ToString() + " min";
                return System.Math.Round(duration.TotalMinutes,1).ToString() + " min";
            }
            if (duration.TotalDays < 1)
            {
                return System.Math.Round(duration.TotalHours).ToString() + " hr";
            }
            #if NET35
            return duration.ToString();
            #else
            return duration.ToString("G");
            #endif
        }

        public static string GetFormattedDateTime(System.DateTime time)
        {
            var now = System.DateTime.Now;
            if (time.Year == now.Year && time.Month == now.Month && time.Day == now.Day)
            {
                return time.ToString("t");
            }
            if(time.Year == now.Year)
            {
                return time.ToString("m");
            }

            return time.ToString("g");
        }

        public static string GetLogDirectory()
        {
            var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) +
                         @"\LEP\CommandHistory";
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir;
        }
        public static string GetLogBaseName(Command command)
        {
            var time = System.DateTime.Now;
            time = command.EndTime;
            var logName = command.Name + "-" + time.ToString("o") + ".Command.bin";
            char[] invalidFileChars = { '/', '\\', ':', '*', '"', '<', '>' };
            foreach (char c in invalidFileChars)
            {
                if (logName.IndexOf(c) > -1) logName = logName.Replace(c, '-');
            }
            return logName;
        }
        public void Save(string filename="")
        {
            if (filename.Length == 0) filename = GetLogDirectory() + @"\" + GetLogBaseName(this);
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                {
                    binaryFormatter.Serialize(fs, this);
                }
            }
        }

        public static Command[] GetHistory(int maxCount,string pattern="")
        {
            var history
                = new System.Collections.Generic.List<Command>();

            System.Text.RegularExpressions.Regex regex = null;
            if(pattern.Length > 0)
            {
                var regexPattern =  "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                regex = new System.Text.RegularExpressions.Regex(regexPattern);

            }
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            foreach (string filename in System.IO.Directory.GetFiles(GetLogDirectory(),"*.Command.bin"))
            {
                var fi = new System.IO.FileInfo(filename);
                if (ReferenceEquals(null, regex) || regex.IsMatch(fi.Name))
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                    {
                        try
                        {
                            var command = binaryFormatter.Deserialize(fs) as Command;
                            history.Add(command);
                        }
                        catch { }
                    }
                }
            }
            history.Sort();
            return history.ToArray();
        }

        public static void ClearHistory(System.TimeSpan ageLimit,string pattern="")
        {
            System.Text.RegularExpressions.Regex regex = null;
            if (pattern.Length > 0)
            {
                var regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                regex = new System.Text.RegularExpressions.Regex(regexPattern);

            }

            foreach (string filename in System.IO.Directory.GetFiles(GetLogDirectory(), "*.Command.bin"))
            {
                var fi = new System.IO.FileInfo(filename);
                if (ReferenceEquals(null, regex) || regex.IsMatch(fi.Name))
                {
                    var age = System.DateTime.Now - fi.CreationTime;
                    if (age > ageLimit)
                    {
                        System.IO.File.Delete(fi.FullName);
                    }
                }
            }
        }
    }
}
