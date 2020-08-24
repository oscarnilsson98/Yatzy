using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace YatzyGrupp7MVVM.Services
{
        public class RelayCommand : ICommand
        {


            private Action<object> action;
            private Func<object, bool> canExecute;

        public RelayCommand(Action<object> action)
        {
            this.action = action;
        }

        public RelayCommand(Action<object> action, Func<object, bool> canExecute)
            {
                this.action = action;
                this.canExecute = canExecute;
            }


            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return canExecute == null || canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged = delegate { };

            public void Execute(object parameter)
            {
                action(parameter);
            }

        public void RaiseCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, new EventArgs());
            }

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        #endregion
    }


}




