using System;
using System.Windows.Input;

namespace CarPrice.ViewModels
{
    public sealed class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> _execute, Func<object, bool> _canExecute = null) => (execute, canExecute) = (_execute, _canExecute);

        public bool CanExecute(object parameter) => canExecute == null || canExecute(parameter);

        public void Execute(object parameter) => execute(parameter);
    }
}
