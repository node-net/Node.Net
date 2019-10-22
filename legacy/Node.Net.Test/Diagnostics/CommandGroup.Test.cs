using NUnit.Framework;
using System.IO;

namespace Node.Net.Diagnostics
{
    [TestFixture]
    class CommandGroupTest
    {
        [Test]
        public void CommandGroup_Usage()
        {
            var commandGroup = new CommandGroup();
            Assert.AreEqual(0, commandGroup.Execute().ExitCode,"empty commandGroup.ExitCode");
            Assert.AreEqual("", commandGroup.Output);

            var directory = GlobalFixture.GetTempPath("CommandGroup_Usage");
            Assert.True(Directory.Exists(directory), "Directory.Exists('{directory}')");
            var rake_dir = $"{directory}\\HelloRake";
            if (Directory.Exists(rake_dir)) FileSystem.Delete(rake_dir);
            commandGroup.Add("clone",
                              new ConsoleCommand
                              { Name = "git clone http://github.com/dev-gem/HelloRake.git", Directory=directory });
            commandGroup.Add("rake",
                                new ConsoleCommand
                                { Name = "rake default", Directory = rake_dir });

            commandGroup.Execute();
            var rake = commandGroup["rake"] as ICommand;
            
            Assert.AreEqual(0, (commandGroup["clone"] as ICommand).ExitCode, "clone.ExitCode");
            Assert.AreEqual(0, rake.ExitCode, "rake.ExitCode");
            Assert.AreEqual(0, commandGroup.ExitCode, "commandGroup.ExitCode");
            Assert.True(Directory.Exists(rake_dir));
            var duration = commandGroup.EndTime - commandGroup.StartTime;
        }
    }
}
