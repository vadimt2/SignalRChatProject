using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFClient.Commands
{
    public class DelagteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<object> ExecuteActionObj { get; }
        Func<bool> ValidateAction { get; }

        public DelagteCommand(Action<object> executeActionObj, Func<bool> validateAction)
        {
            ExecuteActionObj = executeActionObj;
            ValidateAction = validateAction;
        }

        public bool CanExecute(object parameter)
        {
            return ValidateAction();
        }

        public void Execute(object parameter)
        {
            ExecuteActionObj(parameter);
        }

        public void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }
    }
}
