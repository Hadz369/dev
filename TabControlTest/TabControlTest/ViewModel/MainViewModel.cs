using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Dragablz;

namespace TabControlTest
{
    public class MainViewModel : ViewModelBase
    {
        private TabItemViewModel _selectedItem = null;
        private ObservableCollection<NavigationItemViewModel> _reports = new ObservableCollection<NavigationItemViewModel>();
        private ObservableCollection<TabItemViewModel> items = new ObservableCollection<TabItemViewModel>();
        private ObservableCollection<System.Windows.Controls.Button> _buttons = new ObservableCollection<System.Windows.Controls.Button>();
        private List<ReportParameterGroup> _parameterGroups;

        public IInterTabClient InterTabClient { get; set; }

        public MainViewModel()
        {
            InterTabClient = new MainInterTabClient();

            NavigationItemViewModel root = new NavigationItemViewModel(ReportNavigationItemType.Group) { Title = "Report Group #1" };
            NavigationItemViewModel childItem1 = new NavigationItemViewModel(ReportNavigationItemType.Group) { Title = "Sub-Group #1.1" };
            childItem1.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #1.1.1" });
            childItem1.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #1.1.2" });
            root.Children.Add(childItem1);
            root.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #1.1" });
            _reports.Add(root);

            NavigationItemViewModel root2 = new NavigationItemViewModel(ReportNavigationItemType.Group) { Title = "Report Group #2" };
            NavigationItemViewModel childItem2 = new NavigationItemViewModel(ReportNavigationItemType.Group) { Title = "Sub-Group #2.1" };
            childItem2.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #2.1.1" });
            childItem2.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #2.1.2" });
            root2.Children.Add(childItem2);
            root2.Children.Add(new NavigationItemViewModel(ReportNavigationItemType.Report) { Title = "Report #2.1" });
            _reports.Add(root2);

            _parameterGroups = new List<ReportParameterGroup>();
            
            ReportParameterGroup g1 = new ReportParameterGroup("Report Date Range");
            g1.Parameters.Add(new ReportParameterViewModel(ReportParameterType.Date, "DATEFROM", "Date From", new DateTime(2016, 11, 10)));
            g1.Parameters.Add(new ReportParameterViewModel(ReportParameterType.Date, "DATETO",   "Date To",   DateTime.Now.ToString()));
            _parameterGroups.Add(g1);

            ReportParameterGroup g2 = new ReportParameterGroup("Optional Parameters");
            g2.Parameters.Add(new ReportParameterViewModel(ReportParameterType.Numeric, "NUMERIC2", "Integer Parameter 2", 6));
            g2.Parameters.Add(new ReportParameterViewModel(ReportParameterType.String,  "STRING2",  "String Parameter 2",  "ABC"));
            g2.Parameters.Add(new ReportParameterViewModel(ReportParameterType.Date,    "DATE2",    "Date Parameter 2",    new DateTime(2016, 11, 10)));
            Dictionary<int, string> comboItems = new Dictionary<int, string>() {
                {1, "First Item"},
                {2, "Second Item"},
                {3, "Third Item"}
            };
            g2.Parameters.Add(new ReportParameterViewModel(ReportParameterType.Combo, "ComboParm", "Combo Parameter 1", comboItems));
            _parameterGroups.Add(g2);
        }

        public ObservableCollection<NavigationItemViewModel> Reports { get { return _reports; } }

        public ObservableCollection<TabItemViewModel> Items { get { return items; } }

        public List<ReportParameterGroup> ParameterGroups { get { return _parameterGroups; } }

        public ObservableCollection<System.Windows.Controls.Button> Buttons
        {
            get
            {
                return _buttons;
            }
        }

        public TabItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetSelectedItem(value); }
        }

        void SetSelectedItem(TabItemViewModel value)
        {
            _selectedItem = value;
            OnPropertyChanged("SelectedItem");
        }
    }
}