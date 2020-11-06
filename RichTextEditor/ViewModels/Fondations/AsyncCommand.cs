﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RichTextEditor.ViewModels.Fondations
{
	public interface IAsyncCommand: ICommand
	{
		Task ExecuteAsync(object parameter);
		new bool CanExecute(object parameter);
	}
	// https://johnthiriet.com/removing-async-void/
	public interface IErrorHandler
	{
		void HandleError(Exception ex);
	}
	public static class TaskUtilities
	{
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			try
			{
				await task;
			}
			catch(Exception ex)
			{
				handler?.HandleError(ex);
			}
		}
	}

	/// <summary>
	/// AsyncCommand is for command that can be executed asynchronized with proper exception handling technique by John Tririet https://johnthiriet.com/mvvm-going-async-with-async-command/
	/// Execute a command put us in a situation with async void and his method deals with this.
	/// </summary>
	public class AsyncCommand : IAsyncCommand
	{
		public event EventHandler CanExecuteChanged;

		private bool isExecuting;
		private readonly Func<object,Task> execute;
		private readonly Predicate<object> canExecute;
		private readonly IErrorHandler errorHandler;

		public AsyncCommand(Func<object, Task> execute_, Predicate<object> canExecute_ = null, IErrorHandler errorHandler_ = null)
		{
			execute = execute_;
			canExecute = canExecute_??((_)=>{ return true; });
			errorHandler = errorHandler_;
		}

		bool IAsyncCommand.CanExecute(object parameter)
		{
			return !isExecuting && (canExecute?.Invoke(parameter) ?? true);
		}

		bool ICommand.CanExecute(object parameter)
		{
			return (this as IAsyncCommand).CanExecute(parameter);
		}

		/// <summary>
		/// The essence of asyncCommand is this fire and forget task on a different thread called by the actual execute method
		/// </summary>
		/// <param name="parameter"></param>
		void ICommand.Execute(object parameter)
		{
			Task.Run(()=>(this as IAsyncCommand).ExecuteAsync(parameter).FireAndForgetSafeAsync(errorHandler));
		}

		async Task IAsyncCommand.ExecuteAsync(object parameter)
		{
			try
			{
				isExecuting = true;
				await this.execute(parameter);
			}
			finally
			{
				isExecuting = false;
			}
			RaiseCanExecuteChanged();
		}
		public void RaiseCanExecuteChanged()
		{
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
