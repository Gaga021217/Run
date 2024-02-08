using System;
using System.Windows.Input;

namespace Run.ViewModel {
    /// <summary>
    /// Type of delegate command
    /// </summary>
    public class DelegateCommand : ICommand {
        private readonly Action<Object?> _execute; // the lambda executing the action
        private readonly Func<Object?, Boolean>? _canExecute; // lambda checking the condition to execute

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Action to execute</param>
        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canExecute">Condition of execution.</param>
        /// <param name="execute">Action to execute.</param>
        public DelegateCommand(Func<Object?, Boolean>? canExecute, Action<Object?> execute) {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event for the changing of the condition to execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Checking condition to execute
        /// </summary>
        /// <param name="parameter">Paramether of the action.</param>
        /// <returns>True, if the action can be executed, false otherwise.</returns>
        public Boolean CanExecute(Object? parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Executing action
        /// </summary>
        /// <param name="parameter">Paramether of the action.</param>
        public void Execute(Object? parameter) {
            if (!CanExecute(parameter)) {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }

        /// <summary>
        /// Triggering event CanExecuteChanged.
        /// </summary>
        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
