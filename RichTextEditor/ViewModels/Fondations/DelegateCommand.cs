using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RichTextEditor.ViewModels.Fondations
{
    /// <summary>
    /// Simple implementation of icommand for synchronized calls
    /// </summary>
    public class DelegateCommand : ICommand
	{
		public DelegateCommand(Action<object> execute) : this(execute, null)
		{
		}
		public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if(execute == null) throw new ArgumentNullException(nameof(execute));
			_execute = execute;
			_canExecute = canExecute;
		}
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
		public void RaiseCanExecuteChanged()
		{
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
