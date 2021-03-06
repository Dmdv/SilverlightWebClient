using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using WebClient.ICS.Client.Properties;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// An <see cref="ICommand" /> whose delegates can be attached for 
	/// <see cref="Execute" /> and <see cref="CanExecute" /> . It also implements the 
	/// <see cref="IActiveAware" /> interface, which is useful when registering this command in a 
	/// <see cref="CompositeCommand" /> that monitors command's activity.
	/// </summary>
	public sealed class DelegateCommand<T> : ICommand, IActiveAware
	{
		// ReSharper disable MemberCanBePrivate.Global

		private readonly Func<T, bool> _canExecuteMethod;
		private readonly Action<T> _executeMethod;
		private List<WeakReference> _canExecuteChangedHandlers;
		private bool _isActive;

		/// <summary>
		/// 	Initializes a new instance of <see cref="DelegateCommand{T}" /> .
		/// </summary>
		/// <param name="executeMethod"> Delegate to execute when Execute is called on the command. 
		/// This can be null to just hook up a CanExecute delegate. </param>
		/// <param name="canExecuteMethod"> Delegate to execute when CanExecute is called on the command. </param>
		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null)
		{
			if (executeMethod == null && canExecuteMethod == null)
			{
				throw new ArgumentNullException("executeMethod", Resources.DelegateCommandDelegatesCannotBeNull);
			}

			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}

		/// <summary>
		/// 	Fired if the <see cref="IsActive" /> property changes.
		/// </summary>
		public event EventHandler IsActiveChanged;

		/// <summary>
		/// 	Gets or sets a value indicating whether the object is active.
		/// </summary>
		/// <value> <see langword="true" /> if the object is active; otherwise <see langword="false" /> . </value>
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					OnIsActiveChanged();
				}
			}
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute. 
		/// You must keep a hard reference to the handler to avoid garbage collection and unexpected results. 
		/// See remarks for more information.
		/// </summary>
		/// <remarks>
		/// 	When subscribing to the <see cref="ICommand.CanExecuteChanged" /> event using code 
		/// (not when binding using XAML) will need to keep a hard reference to the event handler. 
		/// This is to prevent garbage collection of the event handler because the command implements the 
		/// Weak Event pattern so it does not have a hard reference to this handler. 
		/// An example implementation can be seen in the CompositeCommand and CommandBehaviorBase classes. 
		/// In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, 
		/// but if you do, you are responsible for maintaining the reference.
		/// </remarks>
		/// <example>
		/// The following code holds a reference to the event handler. 
		/// The myEventHandlerReference value should be stored in an instance member 
		/// to avoid it from being garbage collected. 
		/// </example>
		public event EventHandler CanExecuteChanged
		{
			add { WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2); }
			remove { WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value); }
		}

		/// <summary>
		///	Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter"> Data used by the command. 
		/// If the command does not require data to be passed, this object can be set to 
		/// <see langword="null" />.</param>
		/// <returns> <see langword="true" /> if this command can be executed; otherwise, 
		/// <see langword="false" />.</returns>
		public bool CanExecute(T parameter)
		{
			return _canExecuteMethod == null || _canExecuteMethod(parameter);
		}

		/// <summary>
		///	Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter"> Data used by the command. 
		/// If the command does not require data to be passed, this object can be set to 
		/// <see langword="null" /> . </param>
		public void Execute(T parameter)
		{
			if (_executeMethod == null)
			{
				return;
			}
			_executeMethod(parameter);
		}

		/// <summary>
		/// Raises <see cref="CanExecuteChanged" /> on the UI thread so every command invoker 
		/// can requery to check if the command can execute.
		/// <remarks>
		/// Note that this will trigger the execution of <see cref="CanExecute" /> once for each invoker.
		/// </remarks>
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		public void RaiseCanExecuteChanged()
		{
			OnCanExecuteChanged();
		}

		/// <summary>
		///	Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter"> Data used by the command. 
		/// If the command does not require data to be passed, this object can be set to null. </param>
		/// <returns> true if this command can be executed; otherwise, false. </returns>
		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T) parameter);
		}

		/// <summary>
		///	Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter"> Data used by the command. 
		/// If the command does not require data to be passed, this object can be set to null. </param>
		void ICommand.Execute(object parameter)
		{
			Execute((T) parameter);
		}

		/// <summary>
		/// 	Raises <see cref="ICommand.CanExecuteChanged" /> on the UI thread so every command invoker can requery
		/// <see cref="ICommand.CanExecute" /> to check if the <see cref="CompositeCommand" /> can execute.
		/// </summary>
		private void OnCanExecuteChanged()
		{
			WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
		}

		/// <summary>
		/// 	This raises the <see cref="IsActiveChanged" /> event.
		/// </summary>
		private void OnIsActiveChanged()
		{
			var isActiveChangedHandler = IsActiveChanged;

			if (isActiveChangedHandler != null)
			{
				isActiveChangedHandler(this, EventArgs.Empty);
			}
		}
	}
}