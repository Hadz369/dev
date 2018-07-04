using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TabControlTest
{
    public enum ReportNavigationItemType
    {
        Group,
        Report
    }

    public class NavigationItemViewModel : ViewModelBase
    {
        string _title = "";
        bool _isExpanded = true;
        Image _groupImage, _reportImage;

        public NavigationItemViewModel(ReportNavigationItemType itemType)
        {
            ItemType = itemType;
            this.Children = new ObservableCollection<NavigationItemViewModel>();

        }

        public ReportNavigationItemType ItemType { get; private set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetIsExpanded(value); }
        }

        void SetIsExpanded(bool value)
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetTitle(value); }
        }

        void SetTitle(string value)
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public Image Image
        { 
            get { return ItemType == ReportNavigationItemType.Group ? _groupImage : _reportImage; } 
        }

        public ObservableCollection<NavigationItemViewModel> Children { get; set; }
    }
}