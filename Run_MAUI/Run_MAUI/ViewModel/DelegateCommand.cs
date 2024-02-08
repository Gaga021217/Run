using System.Windows.Input;

namespace Run_MAUI.ViewModel {
    public class DelegateCommand : ICommand {
        private readonly Action<Object?> _execute;
        private readonly Func<Object?, bool>? _canExecute;

        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }

        public DelegateCommand(Func<Object?, bool>? canExecute, Action<Object?> execute) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(Object? parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(Object? parameter) {
            if (!CanExecute(parameter)) {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
