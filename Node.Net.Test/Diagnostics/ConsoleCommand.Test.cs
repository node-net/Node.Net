using NUnit.Framework;
using System.IO;

namespace Node.Net.Diagnostics
{
    [TestFixture]
    public class ConsoleCommandTest
    {
        [TestCase("git", 1)]
        [TestCase("git help", 0)]
        public void ConsoleCommand_ExitCode_Check(string name, int expected_exit_code)
        {
            var command = new ConsoleCommand { Name = name };
            command.Execute();
            Assert.AreEqual(expected_exit_code, command.ExitCode, "command.ExitCode");
        }

        [TestCase("git", "commit")]
        [TestCase("git help", "usage:")]
        public void ConsoleCommand_Output_Check(string name, string expected_output_substring)
        {
            var command = new ConsoleCommand { Name = name };
            command.Execute();
            Assert.True(command.Output.Contains(expected_output_substring), $"command.Output did not contain '{expected_output_substring}'");
        }

        [Test]
        public void ConsoleCommand_Git_Checkout()
        {
            const string git_url = "http://github.com/dev-gem/HelloRake.git";
            var parent_dir = GlobalFixture.GetTempPath("ConsoleCommand_Git_Checkout");
            var wrk_dir = $"{parent_dir}\\HelloRake";
            FileSystem.Delete(wrk_dir);

            Assert.True(Directory.Exists(parent_dir), $"{parent_dir} does not exist");
            var clone = new ConsoleCommand { Name = $"git clone {git_url}", Directory = parent_dir };
            clone.Execute();
            Assert.AreEqual(0, clone.ExitCode, $"clone.ExitCode {clone.Error}");
            Assert.True(Directory.Exists(wrk_dir), $"Directory.Exists('wrk_dir')");
        }
    }
}
