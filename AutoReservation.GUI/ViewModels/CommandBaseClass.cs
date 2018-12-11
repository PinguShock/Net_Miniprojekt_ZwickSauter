using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AutoReservation.GUI.ViewModels {
    public class CommandBaseClass : ICommand {

        public CommandBaseClass(Action<object> execute) : this(execute, null) { }

        public CommandBaseClass(Action<object> execute, Predicate<object> canExecute) {
            this.execute = execute;
            this.canExecute = canExecute;
        }



        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter) {
            if (!CanExecute(parameter)) {
                return;
            }
            execute(parameter);
        }
    }
}
