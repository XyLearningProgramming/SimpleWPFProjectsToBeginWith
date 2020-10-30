using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace VeryBasicMVVM
{
	public class RelayCommand : ICommand
	{
		public RelayCommand(Action<object> execute): this(execute, null)
		{
		}
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if(execute == null) throw new ArgumentNullException(nameof(execute));
			_execute = execute;
			_canExecute = canExecute;
		}
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}

			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
