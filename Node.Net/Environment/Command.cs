namespace Node.Net.Environment
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
            Command command = new Command(cmd, timeout);
            command.Execute();
            return command;
        }
        public static Command Execute(string cmd,string workingDirectory,int timeout)
        {
            Command command = new Command(cmd, workingDirectory, timeout);
            command.Execute();
            return command;
        }
        public Command() { StartTime = System.DateTime.Now; this["Type"] = "Command"; }
        public Command(string cmd) { StartTime = System.DateTime.Now; this["Type"] = "Command"; this["Name"] = cmd; }
        public Command(string cmd, string workingDirectory) { StartTime = System.DateTime.Now; this["Type"] = "Command"; this["Name"] = cmd; Directory = workingDirectory; }
        public Command(string cmd, string workingDirectory, int timeout) { StartTime = System.DateTime.Now; this["Type"] = "Command"; this["Name"] = cmd; Directory = workingDirectory; Timeout = timeout; }
        public Command(string cmd, int timeout) { StartTime = System.DateTime.Now; this["Type"] = "Command"; this["Name"] = cmd; Timeout = timeout; }

        private Command(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext context) : base(info,context)
        {
        }

        public string Name
        {
            get { return getString("Name");}
            private set { this["Name"] = value; }
        }

        private string getString(string key) => ContainsKey(key) ? this[key].ToString() : "";

        public string FileName
        {
            get 
            {
                if(!ContainsKey("FileName"))
                {
                    this["FileName"] = GetFileName(Name);
                }
                return getString("FileName");
            }
        }

        public string Directory
        {
            get
            {
                return getString("Directory");
            }
            set
            {
                this["Directory"] = value;
            }
        }

        public int Timeout
        {
            get
            {
                if (ContainsKey("Timeout")) return (int)(double)this["Timeout"];
                return 0;
            }
            set
            {
                this["Timeout"] = (double)value;
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
                if (!ContainsKey("EndTime")) return false;
                return true;
            }
        }
        public int ExitCode
        {
            get
            {
                if (ContainsKey("ExitCode")) return (int)(double)this["ExitCode"];
                return 0;
            }
            set
            {
                this["ExitCode"] = (double)value;
            }
        }

        public string User
        {
            get
            {
                if (ContainsKey("User")) return this["User"].ToString();
                return System.Environment.UserName;
            }
            set { this["User"] = value; }
        }

        public string Machine
        {
            get
            {
                if (ContainsKey("Machine")) return this["Machine"].ToString();
                return System.Environment.MachineName;
            }
            set { this["Machine"] = value; }
        }

        public string Arguments
        {
            get
            {
                string name = Name.Trim();
                string[] parts = name.Split(' ');
                return name.Substring(parts[0].Length).Trim();
            }
        }

        public System.DateTime StartTime
        {
            get
            {
                if (ContainsKey("StartTime"))
                {
                    return System.DateTime.Parse(this["StartTime"].ToString());
                }
                this["StartTime"] = System.DateTime.Now.ToString("o");
                return System.DateTime.Parse(this["StartTime"].ToString());
            }
            set
            {
                this["StartTime"] = value.ToString("o");
            }
        }

        public System.DateTime EndTime
        {
            get
            {
                if(ContainsKey("EndTime"))
                {
                    return System.DateTime.Parse(this["EndTime"].ToString());
                }
                this["EndTime"] = System.DateTime.Now.ToString("o");
                return System.DateTime.Parse(this["EndTime"].ToString());
            }
            set
            {
                this["EndTime"] = value.ToString("o");
            }
        }

        public System.TimeSpan Duration => EndTime - StartTime;

        public string Output
        {
            get
            {
                if (ContainsKey("Output")) return this["Output"].ToString();
                return "";
            }
            set
            {
                this["Output"] = value;
            }
        }

        public void AppendOutput(string value)
        {
            if (Output.Length == 0) Output = value + System.Environment.NewLine;
            else
            {
                Output = Output + value + System.Environment.NewLine;
            }
        }

        public string Error
        {
            get
            {
                if (ContainsKey("Error")) return this["Error"].ToString();
                return "";
            }
            set
            {
                this["Error"] = value;
            }
        }

        public static string GetFileName(string command)
        {
            string target = command;
            
            string[] parts = command.Split(' ');
            if (parts.Length > 1) target = parts[0];

            string fname = target + ".exe";
            if (System.IO.File.Exists(fname)) return fname;

            fname = target + ".bat";
            if (System.IO.File.Exists(fname)) return fname;

            // search path
            string[] allpaths = System.Environment.GetEnvironmentVariable("PATH").Split(';');
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
            if (object.ReferenceEquals(null, other)) return;
            string durationStr = "";
            string status = " not executed";

            if (true)//other.Duration.HasValue)
            {
                durationStr = " [" + GetFormattedTimeSpan(other.Duration) + "]";
                if (other.ExitCode == 0) status = " OK";
                else status = " Error";
            }

            if (other.ExitCode != 0)
            {
                ExitCode = other.ExitCode;
                if (Error.Length == 0) Error = other.Name + durationStr + status;
                else Error = Error + System.Environment.NewLine + durationStr + status;
            }
            if(other.Error.Length > 0)
            {
                if (Error.Length == 0) Error = other.Error;
                else Error = Error + System.Environment.NewLine + other.Error;
            }
            
            if (Output.Length == 0) Output = other.Name + durationStr + status;
            else Output = Output + System.Environment.NewLine + other.Name + durationStr + status;
            if(other.Output.Length > 0)
            {
                if (Output.Length == 0) Output = other.Output;
                else Output = Output + System.Environment.NewLine + other.Output;
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
            if (!object.ReferenceEquals(null, value))
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
            int count = 0;
            int hashCode = 0;
            foreach (object item in value)
            {
                int tmp = GetHashCode(item);
                if (tmp != 0) count++;
                hashCode = hashCode ^ tmp;
            }
            hashCode = hashCode ^ count;
            return hashCode;
        }
        public static int GetHashCode(System.Collections.IDictionary value)
        {
            int hashCode = value.Count;
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
            if (object.ReferenceEquals(this, value)) return 0;
            if (object.ReferenceEquals(null, value)) return 1;

            Command thatCommand = value as Command;
            if (!object.ReferenceEquals(null, thatCommand))
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
            System.DateTime now = System.DateTime.Now;
            if(time.Year == now.Year && time.Month == now.Month && time.Day == now.Day)
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
            string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) +
                         @"\LEP\CommandHistory";
            if(!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir;
        }
        public static string GetLogBaseName(Command command)
        {
            System.DateTime time = System.DateTime.Now;
            time = command.EndTime;
            string logName = command.Name + "-" + time.ToString("o") + ".Command.bin";
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
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                {
                    binaryFormatter.Serialize(fs, this);
                }
            }
        }

        public static Command[] GetHistory(int maxCount,string pattern="")
        {
            System.Collections.Generic.List<Command> history
                = new System.Collections.Generic.List<Command>();

            System.Text.RegularExpressions.Regex regex = null;
            if(pattern.Length > 0)
            {
                string regexPattern =  "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                regex = new System.Text.RegularExpressions.Regex(regexPattern);

            }
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            foreach(string filename in System.IO.Directory.GetFiles(GetLogDirectory(),"*.Command.bin"))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                if (object.ReferenceEquals(null, regex) || regex.IsMatch(fi.Name))
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                    {
                        try
                        {
                            Command command = binaryFormatter.Deserialize(fs) as Command;
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
                string regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                regex = new System.Text.RegularExpressions.Regex(regexPattern);

            }

            foreach (string filename in System.IO.Directory.GetFiles(GetLogDirectory(), "*.Command.bin"))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                if (object.ReferenceEquals(null, regex) || regex.IsMatch(fi.Name))
                {
                    System.TimeSpan age = System.DateTime.Now - fi.CreationTime;
                    if (age > ageLimit)
                    {
                        System.IO.File.Delete(fi.FullName);
                    }
                }
            }
        }
    }
}
