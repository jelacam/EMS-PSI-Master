﻿using System;
using System.Windows.Input;

namespace UIClient
{
	public class RelayCommand:RelayCommand<object>
	{
		#region Constructors

		public RelayCommand(Action<object> execute)
			: base(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
			: base(execute, canExecute)
		{
		}

		#endregion // Constructors
	}

	public class RelayCommand<T> : ICommand
	{
		#region Fields

		private Action<T> execute;
		private Predicate<T> canExecute;
		
		#endregion // Fields

		#region Constructors

		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}
		#endregion // Constructors

		#region Events

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}

			remove
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		/// <summary>
		/// Fired if the <see cref="IsActive"/> property changes.
		/// </summary>
		public event EventHandler IsActiveChanged;

		#endregion // Events

		#region Properties
	
		public void SetCanExecute(Predicate<T> method)
		{
			canExecute = method;
		}

		public void SetExecute(Action<T> method)
		{
			execute = method;
		}
		#endregion // Properties

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return this.canExecute == null ? true : this.canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			//TODO Logging
			this.execute((T)parameter);
		}

		#endregion // ICommand Members

	}

}
