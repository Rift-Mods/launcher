using System;
using System.Windows.Input;

namespace launcher.LauncherUI
{
    class RCommand : ICommand
    {
        private Action<object> _exec;
        private Func<object, bool> _canDo;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RCommand(Action<object> exec, Func<object, bool> canDo = null)
        {
            _exec = exec;
            _canDo = canDo;
        }

        public bool CanExecute(object parameter)
        {
            return _canDo == null || _canDo(parameter);
        }

        public void Execute(object parameter)
        {
            _exec(parameter);
        }
    }
}
