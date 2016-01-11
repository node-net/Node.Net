using System.Windows;

namespace Node.Net.Framework
{
    public class Application
    {
        private Window mainWindow = null;
        public Window MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }

        public virtual int Run(string[] args)
        {
            if (!ReferenceEquals(null, MainWindow))
            {
                MainWindow.ShowDialog();
            }
            return 0;
        }

        public int Run(string args)
        {
            return Run(args.Split(' '));
        }

        public int Run() { return Run(""); }

    }
}
