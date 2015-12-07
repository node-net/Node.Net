namespace Node.Net.Environment
{
    [NUnit.Framework.TestFixture, NUnit.Framework.Category("Node.Net.Environment.Command")]
    public class Command_Test : System.Collections.Generic.Dictionary<string,Command>
    {
        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            Clear();
            Add("default", new Command());
            Add("ruby_version", new Command("ruby --version"));
            Add("git_help", new Command("git help",
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)));
            Add("ftp", new Command("ftp", 100));
            Add("git_bogus", new Command("git bogus"));
        }

        [NUnit.Framework.TestCase]
        public void Command_Not_Executed()
        {
            Command cmd = new Command("ruby --version");
            NUnit.Framework.Assert.False(cmd.HasExecuted);
        }
        [NUnit.Framework.TestCase]
        public void Command_Usage()
        {
            NUnit.Framework.Assert.AreEqual("ruby --version", this["ruby_version"].Name);
            NUnit.Framework.Assert.True(System.IO.File.Exists(this["ruby_version"].FileName));
            NUnit.Framework.Assert.AreEqual("", this["ruby_version"].Directory);
            NUnit.Framework.Assert.AreEqual(0, this["ruby_version"].Timeout);
            //Assert.False(this["ruby_version"].StartTime.HasValue);
            //Assert.False(this["ruby_version"].EndTime.HasValue);
            //Assert.False(this["ruby_version"].Duration.HasValue);
            NUnit.Framework.Assert.AreEqual("", this["ruby_version"].Output);
            NUnit.Framework.Assert.AreEqual("", this["ruby_version"].Error);
            NUnit.Framework.Assert.AreEqual(0, this["ruby_version"].ExitCode);
            this["ruby_version"].Execute();
            NUnit.Framework.Assert.AreEqual(0, this["ruby_version"].ExitCode);
            NUnit.Framework.Assert.True(this["ruby_version"].Output.Contains("ruby"));
            //Assert.True(this["ruby_version"].StartTime.HasValue);
            //Assert.True(this["ruby_version"].EndTime.HasValue);
            //Assert.True(this["ruby_version"].Duration.HasValue);

            string start = this["ruby_version"].StartTime.ToString();
            string end = this["ruby_version"].EndTime.ToString();
            NUnit.Framework.Assert.AreNotEqual(0, this["ruby_version"].Duration.Milliseconds);

            NUnit.Framework.Assert.AreEqual(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                            this["git_help"].Directory);

            NUnit.Framework.Assert.True(System.IO.File.Exists(this["ftp"].FileName));
            NUnit.Framework.Assert.AreEqual("", this["ftp"].Directory);
            NUnit.Framework.Assert.AreEqual(100, this["ftp"].Timeout);

            NUnit.Framework.Assert.AreEqual("bogus", this["git_bogus"].Arguments);
            this["git_bogus"].Execute();
            NUnit.Framework.Assert.True(this["git_bogus"].Error.Contains("not"));

            Command cmd = new Command("svn info");
            NUnit.Framework.Assert.AreNotEqual("", cmd.Machine);
            NUnit.Framework.Assert.AreNotEqual("", cmd.User);
            //Assert.False(cmd.Duration.HasValue);
        }

        [NUnit.Framework.TestCase, NUnit.Framework.Explicit]
        public void Command_TimedOut()
        {
            Command ftp = Command.Execute("ftp", 300);
            NUnit.Framework.Assert.AreNotEqual(0, ftp.ExitCode);
            NUnit.Framework.Assert.True(ftp.Error.Contains("Timed out"));
        }
        [NUnit.Framework.TestCase]
        public void Command_Timeout_On_Success()
        {
            Command git_help = Command.Execute("git help", 30000);
            NUnit.Framework.Assert.True(git_help.Output.Contains("push"));
            NUnit.Framework.Assert.True(git_help.Duration.TotalSeconds < 3);
        }

        [NUnit.Framework.TestCase]
        public void Command_Timeout_On_Failure()
        {
            Command git_bogus = Command.Execute("git bogus", 30000);
            NUnit.Framework.Assert.AreNotEqual(0, git_bogus.ExitCode, "`git bogus` exit code was " + git_bogus.ExitCode);

            NUnit.Framework.Assert.True(git_bogus.Duration.TotalSeconds < 10, "`git bogus` duration was " + git_bogus.Duration.TotalSeconds + " seconds.");
        }

        [NUnit.Framework.TestCase]
        public void Command_GetFileName()
        {
            NUnit.Framework.Assert.True(System.IO.File.Exists(Command.GetFileName("ruby")));
            NUnit.Framework.Assert.True(System.IO.File.Exists(Command.GetFileName("ruby.exe")));
            NUnit.Framework.Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("ruby"))));
            NUnit.Framework.Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("ruby").Replace(".exe",""))));

            NUnit.Framework.Assert.True(System.IO.File.Exists(Command.GetFileName("rake")));
            NUnit.Framework.Assert.True(System.IO.File.Exists(Command.GetFileName("rake.bat")));
            NUnit.Framework.Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("rake"))));
            NUnit.Framework.Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("rake").Replace(".bat", ""))));
        }

        [NUnit.Framework.TestCase]
        public void Command_Rake()
        {
            string rakefile = System.IO.Path.GetTempPath() + @"\rakefile.rb";
            if (System.IO.File.Exists(rakefile)) System.IO.File.Delete(rakefile);
            using(System.IO.StreamWriter sw = new System.IO.StreamWriter(rakefile))
            {
                sw.WriteLine("task :default do");
                sw.WriteLine("  puts 'hello rake'");
                sw.WriteLine("end");
            }
            System.IO.FileInfo fi = new System.IO.FileInfo(rakefile);
            Command rake_default = Command.Execute("rake default", fi.DirectoryName, 0);
            NUnit.Framework.Assert.True(rake_default.Output.Contains("hello rake"));
            NUnit.Framework.Assert.AreNotEqual(0, rake_default.Machine.Length);
            NUnit.Framework.Assert.AreNotEqual(0, rake_default.User.Length);
            if (System.IO.File.Exists(rakefile)) System.IO.File.Delete(rakefile);
        }

        [NUnit.Framework.TestCase]
        public void Command_Logging()
        {
            System.Collections.Generic.Dictionary<string, object> log
                = new System.Collections.Generic.Dictionary<string, object>();
            log.Add(log.Count.ToString().PadLeft(3,'0'), Command.Execute("ruby --version",3000));
            log.Add(log.Count.ToString().PadLeft(3, '0'), Command.Execute("svn --version", 3000));
            log.Add(log.Count.ToString().PadLeft(3, '0'), Command.Execute("git --version", 3000));
        }

        [NUnit.Framework.TestCase]
        public void Command_Merge()
        {
            Command cmd = new Command("top");
            Command bogus = new Command("bogus");
            bogus.Execute();
            NUnit.Framework.Assert.AreNotEqual(0, bogus.ExitCode);
            cmd.Merge(bogus);
            NUnit.Framework.Assert.AreNotEqual(0, cmd.ExitCode);
            NUnit.Framework.Assert.True(cmd.Error.Contains(bogus.Error));
        }

        [NUnit.Framework.TestCase]
        public void Command_Serialization()
        {
            Command cmd = new Command("ruby --version");
            cmd.Execute();
            string filename = System.IO.Path.GetTempPath() + "command.bin";
            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binary = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                binary.Serialize(fs, cmd);
            }
            NUnit.Framework.Assert.True(System.IO.File.Exists(filename));

            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Open))
            {
                Command cmd2 = binary.Deserialize(fs) as Command;
                NUnit.Framework.Assert.NotNull(cmd2);
                NUnit.Framework.Assert.AreEqual(cmd.Name, cmd2.Name);
                NUnit.Framework.Assert.AreEqual(0, cmd.CompareTo(cmd2));
            }
            System.IO.File.Delete(filename);
        }
        [NUnit.Framework.TestCase]
        public void Command_Save()
        {
            Command cmd = Command.Execute("ruby --version", 3000);
            cmd.Save();
            System.Collections.Generic.List<Command> history =
                new System.Collections.Generic.List<Command>(Command.GetHistory(10));
            NUnit.Framework.Assert.True(history.Contains(cmd), "Command is not present in the history");

            Command cmd2 = Command.Execute("svn --version", 3000);
            cmd2.Save();
            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10));
            NUnit.Framework.Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            NUnit.Framework.Assert.True(history.Contains(cmd2), "Command cmd2 is not present in the history");

            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10,"ruby*"));
            NUnit.Framework.Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            NUnit.Framework.Assert.False(history.Contains(cmd2), "Command cmd2 is not present in the history");

            Command cmd3 = Command.Execute("ruby -v", 3000);
            cmd3.Save();
            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10, "ruby*"));
            NUnit.Framework.Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            NUnit.Framework.Assert.False(history.Contains(cmd2), "Command cmd2 is not present in the history");
            NUnit.Framework.Assert.True(history.Contains(cmd3), "Command cmd is not present in the history");
            NUnit.Framework.Assert.True(history.IndexOf(cmd3) > -1);
            NUnit.Framework.Assert.True(history.IndexOf(cmd) > -1);
            NUnit.Framework.Assert.True(history.IndexOf(cmd3) < history.IndexOf(cmd));

            Command.ClearHistory(new System.TimeSpan(30, 0, 0, 0));  // 30 days
        }

    }
}
