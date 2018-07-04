using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TabControlTest
{
    public class ReportViewModel : ViewModelBase
    {
        public ReportViewModel(string reportName)
        {
            ReportName = reportName;
            ReportParameterGroups = new ObservableCollection<ReportParameterGroupViewModel>();
        }

        public string ReportName { get; private set; }

        public ObservableCollection<ReportParameterGroupViewModel> ReportParameterGroups { get; private set; }

        public string ReportDefinition { get { return "Reports\\Report2.rdlc"; } }
    }
}
