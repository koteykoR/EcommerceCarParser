using System;
using System.Windows.Input;

namespace CarPrice.Helpers
{
    public sealed class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> _execute, Predicate<object> _canExecute = null) => (execute, canExecute) = (_execute, _canExecute);

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);
    }
}
