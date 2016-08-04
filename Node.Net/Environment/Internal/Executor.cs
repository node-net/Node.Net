namespace Node.Net.Environment.Internal
{
    public class Executor : System.IDisposable
    {
        private Command command;
        private int pid;
        System.Threading.AutoResetEvent autoEvent = new System.Threading.AutoResetEvent(false);
        
        public Executor(Command value) { command = value; }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (autoEvent != null)
                {
                    #if NET35
                    autoEvent=null;
                    #else
                    autoEvent.Dispose();
                    #endif
                }
            }
        }
        #endregion

        public int Execute()
        {
            var entry = new System.Threading.ThreadStart(this.ExecutionThread);
            var thread = new System.Threading.Thread(entry);
            thread.Start();

            if (command.Timeout > 0)
            {
                var timeout = new System.TimeSpan(0, 0, 0, 0, command.Timeout);
                if (autoEvent.WaitOne(timeout,false))
                {
                    #if NET35
                    autoEvent=null;
                    #else
                    autoEvent.Dispose();
                    #endif
                    return command.ExitCode;
                }
                else
                {
                    // Timed Out
                    KillProcessAndChildren(pid);
                    thread.Abort();
                    command.EndTime = System.DateTime.Now;
                    command.ExitCode = 55;
                    if (command.Error.Length == 0) command.Error = "Timed out at " + command.Timeout.ToString() + " ms";
                    else command.Error = command.Error + "Timed out at " + command.Timeout.ToString() + " ms";
                }
            }
            else
            {
                thread.Join();
            }

            #if NET35
            autoEvent=null;
            #else
            autoEvent.Dispose();
            #endif
            return command.ExitCode;
        }

        private static void KillProcessAndChildren(int pid)
        {
            var searcher = new System.Management.ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            var moc = searcher.Get();
            foreach (System.Management.ManagementObject mo in moc)
            {
                KillProcessAndChildren(System.Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                var proc = System.Diagnostics.Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (System.ArgumentException)
            {
                // Process already exited.
            }
        }

        public void ExecutionThread()
        {
            command.StartTime = System.DateTime.Now;
            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = command.FileName;

            command.Machine = System.Environment.MachineName;
            command.User = System.Environment.UserName;

            if (command.Directory.Length > 0)
            {
                process.StartInfo.WorkingDirectory = command.Directory;
            }
            process.StartInfo.Arguments = command.Arguments;
            try
            {
                process.Start();
                pid = process.Id;
                if(command.Output.Length > 0)
                {
                    command.Output = command.Output + System.Environment.NewLine + process.StandardOutput.ReadToEnd();
                }
                else command.Output = process.StandardOutput.ReadToEnd();
                if(command.Error.Length > 0)
                {
                    command.Error = command.Error + System.Environment.NewLine + process.StandardError.ReadToEnd();
                }
                else command.Error = process.StandardError.ReadToEnd();
                if (!process.HasExited) process.WaitForExit();


                command.ExitCode = process.ExitCode;
            }
            catch (System.Exception e)
            {
                command.Error = e.ToString();
                command.ExitCode = 1;
            }

            command.EndTime = System.DateTime.Now;
            autoEvent.Set();
        }
    }
}
