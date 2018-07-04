using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using M1.Module.Common.Shared.Commands;

namespace TabControlTest
{
    public enum TabContentType
    {
        ReportViewer,
        Crystal,
        DataGrid,
    }

    public class DelegateCommand : ICommand
    {
        bool _canExecute;

        private readonly Action<object> _command;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> command) : this(command, false) { }

        public DelegateCommand(Action<object> command, bool isEnabled)
        {
            _command = command;
            _canExecute = isEnabled;
        }

        public bool Enabled
        {
            get 
            { 
                return _canExecute; 
            }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    RaiseCanExecuteChanged();
                }
            }
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _command(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public class TabItemViewModel : ViewModelBase
    {
        private string name;
        private bool isSelected;

        public event EventHandler CloseEvent;

        public TabItemViewModel(string tabName, TabContentType contentType)
        {
            Name = tabName;
            ContentType = contentType;
        }

        public TabContentType ContentType { get; private set; }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }

        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { SetIsSelected(value); }
         }

        private void SetIsSelected(bool value)
        {
            if (isSelected != value)
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public ICommand CloseTabCommand
        {
            get { return new DelegateCommand(Close, true); }
        }

        void Close(object parameter)
        {
            if (CloseEvent != null)
                CloseEvent(this, null);
        }
    }
}
