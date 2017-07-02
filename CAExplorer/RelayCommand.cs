// in this file is the implementation of RelayCommand.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CAExplorerNamespace
{
    class RelayCommand : ICommand
    {
        private Action<object> _action;

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameterIn)
        {
            if (parameterIn != null)
            {
                _action(parameterIn);
            }
            else
            {
                _action("without input parameter");
            }
        }

        #endregion
    }
}
