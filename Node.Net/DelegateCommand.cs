using System;
using System.Windows.Input;

namespace Node.Net
{
    /// <summary>
    /// Delegate command
    /// </summary>
    public sealed class DelegateCommand : ICommand
    {
        private readonly Action<object> executeMethod = DoNothing;
        private readonly Func<object, bool> canExecuteMethod = DefaultCanExecute;

        private static void DoNothing(object i)
        {
        }

        private static bool DefaultCanExecute(object i)
        {
            return true;
        }

        /// <summary>
        /// CanExecuteChanged
        /// </summary>
        public event EventHandler? CanExecuteChanged;
        public bool ExecuteEnabled
        {
            get { return _canExecute; }
            set
            {
                _canExecute = value;
                if (CanExecuteChanged != null) { CanExecuteChanged(this, EventArgs.Empty); }
            }
        }
        private bool _canExecute = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeMethod"></param>
        public DelegateCommand(Action<object> executeMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = DefaultCanExecute;
        }

        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod == null)
            {
                return true;
            }

            return this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            if (executeMethod == null)
            {
                return;
            }

            this.executeMethod(parameter);
        }
    }
}