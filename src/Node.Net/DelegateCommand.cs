using System;
using System.Windows.Input;

namespace Node.Net
{
	/// <summary>
	/// Delegate command
	/// </summary>
	public sealed class DelegateCommand : ICommand
	{
		private readonly Action<object> executeMethod = null;
		private readonly Func<object, bool> canExecuteMethod = null;

		/// <summary>
		/// CanExecuteChanged
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { return; }
			remove { return; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="executeMethod"></param>
		public DelegateCommand(Action<object> executeMethod)
		{
			this.executeMethod = executeMethod;
			this.canExecuteMethod = null;
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