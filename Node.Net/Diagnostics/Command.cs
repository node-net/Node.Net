using System;
using System.Collections.Generic;
using static System.Environment;
using static System.Math;

namespace Node.Net.Diagnostics
{
    public abstract class Command : Dictionary<string, dynamic>, ICommand
    {
        public abstract int Execute();
        public int ExitCode
        {
            get { return IDictionaryExtension.Get<int>(this, nameof(ExitCode), 0); }
            set { IDictionaryExtension.Set(this, nameof(ExitCode), value); }
        }

        public string Name
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(Name), string.Empty); }
            set { IDictionaryExtension.Set(this, nameof(Name), value); }
        }

        public string Output
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(Output), string.Empty); }
            set { IDictionaryExtension.Set(this, nameof(Output), value); }
        }

        public string Error
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(Error), string.Empty); }
            set { IDictionaryExtension.Set(this, nameof(Error), value); }
        }

        public int Timeout
        {
            get { return IDictionaryExtension.Get<int>(this, nameof(Timeout), 0); }
            set { IDictionaryExtension.Set(this, nameof(Timeout), value); }
        }

        public DateTime StartTime
        {
            get { return IDictionaryExtension.Get<DateTime>(this, nameof(StartTime), DateTime.MinValue); }
            set
            { IDictionaryExtension.Set(this, nameof(StartTime), value.ToString("o")); }
        }

        public DateTime EndTime
        {
            get { return IDictionaryExtension.Get<DateTime>(this, nameof(EndTime), DateTime.MinValue); }
            set
            { IDictionaryExtension.Set(this, nameof(EndTime), value.ToString("o")); }
        }

        public string Directory
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(Directory), string.Empty); }
            set { IDictionaryExtension.Set(this, nameof(Directory), value); }
        }

        public string User
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(User), UserName); }
            set { IDictionaryExtension.Set(this, nameof(User), value); }
        }

        public string Machine
        {
            get { return IDictionaryExtension.Get<string>(this, nameof(Machine), MachineName); }
            set { IDictionaryExtension.Set(this, nameof(Machine), value); }
        }

        public string FileName
        {
            get
            {
                var filename = IDictionaryExtension.Get<string>(this, nameof(FileName), string.Empty);
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

        public string Duration
        {
            get
            {
                var duration = EndTime - StartTime;
                if (duration.TotalMinutes >= 10) return $"{Round(duration.TotalSeconds / 60, 0)} m";
                if (duration.TotalSeconds > 60) return $"{Round(duration.TotalSeconds / 60.0, 1)} m";
                return $"{Round(duration.TotalSeconds, 0)} s";
            }
        }
        public string Summary
        {
            get
            {
                return Directory.Length == 0 ? $"{ExitCode} {Name} {Duration}" : $"{ExitCode} {Name} {Duration} ({Directory})";
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
    }
}
