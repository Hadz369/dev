using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TabControlTest
{
    public class ReportParameterGroupViewModel : ViewModelBase
    {
        public ReportParameterGroupViewModel(string groupName)
        {
            GroupName = groupName;
            Parameters = new ObservableCollection<ReportParameterViewModel>();
        }

        public string GroupName { get; private set; }

        public ObservableCollection<ReportParameterViewModel> Parameters { get; private set; }
    }
}
