using NUnit.Framework;
namespace Node.Net.Deprecated.Environment
{
    [TestFixture, NUnit.Framework.Category("Node.Net.Environment.Command")]
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

        [TestCase]
        public void Command_Not_Executed()
        {
            var cmd = new Command("ruby --version");
            Assert.False(cmd.HasExecuted);
        }
        [TestCase]
        public void Command_Usage()
        {
            Assert.AreEqual("ruby --version", this["ruby_version"].Name);
            Assert.True(System.IO.File.Exists(this["ruby_version"].FileName));
            Assert.AreEqual("", this["ruby_version"].Directory);
            Assert.AreEqual(0, this["ruby_version"].Timeout);
            //Assert.False(this["ruby_version"].StartTime.HasValue);
            //Assert.False(this["ruby_version"].EndTime.HasValue);
            //Assert.False(this["ruby_version"].Duration.HasValue);
            Assert.AreEqual("", this["ruby_version"].Output);
            Assert.AreEqual("", this["ruby_version"].Error);
            Assert.AreEqual(0, this["ruby_version"].ExitCode);
            this["ruby_version"].Execute();
            Assert.AreEqual(0, this["ruby_version"].ExitCode);
            Assert.True(this["ruby_version"].Output.Contains("ruby"));
            //Assert.True(this["ruby_version"].StartTime.HasValue);
            //Assert.True(this["ruby_version"].EndTime.HasValue);
            //Assert.True(this["ruby_version"].Duration.HasValue);

            var start = this["ruby_version"].StartTime.ToString();
            var end = this["ruby_version"].EndTime.ToString();
            Assert.AreNotEqual(0, this["ruby_version"].Duration.Milliseconds);

            Assert.AreEqual(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                            this["git_help"].Directory);

            Assert.True(System.IO.File.Exists(this["ftp"].FileName));
            Assert.AreEqual("", this["ftp"].Directory);
            Assert.AreEqual(100, this["ftp"].Timeout);

            Assert.AreEqual("bogus", this["git_bogus"].Arguments);
            this["git_bogus"].Execute();
            Assert.True(this["git_bogus"].Error.Contains("not"));

            var cmd = new Command("svn info");
            Assert.AreNotEqual("", cmd.Machine);
            Assert.AreNotEqual("", cmd.User);
            //Assert.False(cmd.Duration.HasValue);
        }

        [TestCase, NUnit.Framework.Explicit]
        public void Command_TimedOut()
        {
            var ftp = Command.Execute("ftp", 300);
            Assert.AreNotEqual(0, ftp.ExitCode);
            Assert.True(ftp.Error.Contains("Timed out"));
        }
        [TestCase]
        public void Command_Timeout_On_Success()
        {
            var git_help = Command.Execute("git help", 30000);
            Assert.True(git_help.Output.Contains("push"));
            Assert.True(git_help.Duration.TotalSeconds < 3);
        }

        [TestCase]
        public void Command_Timeout_On_Failure()
        {
            var git_bogus = Command.Execute("git bogus", 30000);
            Assert.AreNotEqual(0, git_bogus.ExitCode, "`git bogus` exit code was " + git_bogus.ExitCode);

            Assert.True(git_bogus.Duration.TotalSeconds < 10, "`git bogus` duration was " + git_bogus.Duration.TotalSeconds + " seconds.");
        }

        [TestCase]
        public void Command_GetFileName()
        {
            Assert.True(System.IO.File.Exists(Command.GetFileName("ruby")));
            Assert.True(System.IO.File.Exists(Command.GetFileName("ruby.exe")));
            Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("ruby"))));
            Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("ruby").Replace(".exe",""))));

            Assert.True(System.IO.File.Exists(Command.GetFileName("rake")));
            Assert.True(System.IO.File.Exists(Command.GetFileName("rake.bat")));
            Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("rake"))));
            Assert.True(System.IO.File.Exists(
                        Command.GetFileName(Command.GetFileName("rake").Replace(".bat", ""))));
        }

        [TestCase]
        public void Command_Rake()
        {
            var rakefile = System.IO.Path.GetTempPath() + @"\rakefile.rb";
            if (System.IO.File.Exists(rakefile)) System.IO.File.Delete(rakefile);
            using(System.IO.StreamWriter sw = new System.IO.StreamWriter(rakefile))
            {
                sw.WriteLine("task :default do");
                sw.WriteLine("  puts 'hello rake'");
                sw.WriteLine("end");
            }
            var fi = new System.IO.FileInfo(rakefile);
            var rake_default = Command.Execute("rake default", fi.DirectoryName, 0);
            Assert.True(rake_default.Output.Contains("hello rake"));
            Assert.AreNotEqual(0, rake_default.Machine.Length);
            Assert.AreNotEqual(0, rake_default.User.Length);
            if (System.IO.File.Exists(rakefile)) System.IO.File.Delete(rakefile);
        }

        [TestCase]
        public void Command_Logging()
        {
            var log
                = new System.Collections.Generic.Dictionary<string, object>();
            log.Add(log.Count.ToString().PadLeft(3,'0'), Command.Execute("ruby --version",3000));
            log.Add(log.Count.ToString().PadLeft(3, '0'), Command.Execute("svn --version", 3000));
            log.Add(log.Count.ToString().PadLeft(3, '0'), Command.Execute("git --version", 3000));
        }

        [TestCase]
        public void Command_Merge()
        {
            var cmd = new Command("top");
            var bogus = new Command("bogus");
            bogus.Execute();
            Assert.AreNotEqual(0, bogus.ExitCode);
            cmd.Merge(bogus);
            Assert.AreNotEqual(0, cmd.ExitCode);
            Assert.True(cmd.Error.Contains(bogus.Error));
        }

        [TestCase]
        public void Command_Serialization()
        {
            var cmd = new Command("ruby --version");
            cmd.Execute();
            var filename = System.IO.Path.GetTempPath() + "command.bin";
            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);

            var binary = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                binary.Serialize(fs, cmd);
            }
            Assert.True(System.IO.File.Exists(filename));

            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Open))
            {
                var cmd2 = binary.Deserialize(fs) as Command;
                Assert.NotNull(cmd2);
                Assert.AreEqual(cmd.Name, cmd2.Name);
                Assert.AreEqual(0, cmd.CompareTo(cmd2));
            }
            System.IO.File.Delete(filename);
        }
        [TestCase]
        public void Command_Save()
        {
            var cmd = Command.Execute("ruby --version", 3000);
            cmd.Save();
            var history =
                new System.Collections.Generic.List<Command>(Command.GetHistory(10));
            Assert.True(history.Contains(cmd), "Command is not present in the history");

            var cmd2 = Command.Execute("svn --version", 3000);
            cmd2.Save();
            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10));
            Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            Assert.True(history.Contains(cmd2), "Command cmd2 is not present in the history");

            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10,"ruby*"));
            Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            Assert.False(history.Contains(cmd2), "Command cmd2 is not present in the history");

            var cmd3 = Command.Execute("ruby -v", 3000);
            cmd3.Save();
            history = new System.Collections.Generic.List<Command>(Command.GetHistory(10, "ruby*"));
            Assert.True(history.Contains(cmd), "Command cmd is not present in the history");
            Assert.False(history.Contains(cmd2), "Command cmd2 is not present in the history");
            Assert.True(history.Contains(cmd3), "Command cmd is not present in the history");
            Assert.True(history.IndexOf(cmd3) > -1);
            Assert.True(history.IndexOf(cmd) > -1);
            Assert.True(history.IndexOf(cmd3) < history.IndexOf(cmd));

            Command.ClearHistory(new System.TimeSpan(30, 0, 0, 0));  // 30 days
        }

    }
}
