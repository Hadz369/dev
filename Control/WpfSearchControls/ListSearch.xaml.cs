using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace WpfSearchControls
{
    public delegate void ValueChangedDelegate(object sender);

    /// <summary>
    /// Interaction logic for ListSearch.xaml
    /// </summary>
    public partial class ListSearch : UserControl, IDisposable
    {
        #region Variable Declarations

        bool _textChanged = false, _useTimer = false, _timerActive = false, _autoFilter = true, _initialised = false;
        bool _valChanged = false;

        int _timerInterval = 150, _textChangedDelay = 300;
        DateTime _lastFilterTime, _lastTextChanged;

        string _filter = "", _prevFilter = "";

        iSearchItem _selectedItem, _acceptedItem;
        GridView _view;

        Timer _filterTimer;
        Thread _thread;

        List<string> _searchFields;
        List<iSearchItem> _allitems;

        #endregion

        #region Event Declarations

        public event ValueChangedDelegate Changed;

        #endregion

        #region Initialisation

        public ListSearch()
        {
            InitializeComponent();

            listGrid.MinWidth = tbFilter.Width;

            SetListVisibility(System.Windows.Visibility.Collapsed);
            
            // Subscribe to events
            tbFilter.TextChanged += tbFilter_TextChanged;
            tbFilter.GotFocus += tbFilter_GotFocus;

            tbFilter.PreviewKeyDown += tbFilter_PreviewKeyDown;
            listViewData.PreviewKeyDown += list_PreviewKeyDown;

            this.SizeChanged += ListSearch_SizeChanged;
            this.GotFocus += ListSearch_GotFocus;
            this.LostFocus += ListSearch_LostFocus;
        }

        /// <summary>
        /// Initialise the search object with a list. Text field will be blank and the list will
        /// contain one column containing the text from the ToString() method of the object.
        /// </summary>
        /// <param name="items"></param>
        public void Init()
        {
            Init(null, null, null, false);
        }

        /// <summary>
        /// Initialise the search object with a list. Text field will be blank and the list will
        /// contain one column containing the text from the ToString() method of the object.
        /// </summary>
        /// <param name="items"></param>
        public void Init(List<iSearchItem> items)
        {
            Init(items, null, null, false);
        }

        /// <summary>
        /// Initialise the search object with a list and the currently selected text value. The list will
        /// contain one column containing the text from the ToString() method of the object and the
        /// text field will be populated with the text value.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="text"></param>
        public void Init(List<iSearchItem> items, iSearchItem selectedItem)
        {
            Init(items, selectedItem, null, false);
        }
        /// <summary>
        /// Initialise the search object with a list and a GridView object specifying the columns
        /// to display, as well as the initially selected item. The list will display multiple columns.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="text"></param>
        /// <param name="view"></param>
        public void Init(List<iSearchItem> items, iSearchItem selectedItem, GridView view)
        {
            Init(items, selectedItem, view, false);
        }

        /// <summary>
        /// Initialise the search object with a list and a GridView object specifying the columns
        /// to display, as well as the initially selected item. The list will display multiple columns.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="text"></param>
        /// <param name="view"></param>
        public void Init(List<iSearchItem> items, iSearchItem selectedItem, GridView view, bool noFocus)
        {
            _textChanged = false;

            // Ensure the list and view are empty
            listViewData.ItemsSource = null;
            listViewData.View = null;

            // Set all item placeholders to the initial selection
            this.Tag = _acceptedItem = _selectedItem = selectedItem;
            
            // Required for the internal reset function
            this._view = view;
            
            // Required for the internal reset function and the FindItemByValue function
            this._allitems = items;

            // Set the filter text and ensure the list is hidden
            SetFilterText(selectedItem != null ? selectedItem.Value : String.Empty);
            SetListVisibility(Visibility.Collapsed);

            // If the list has items perform list initialisation and configure the timer 
            if (items != null && items.Count > 0)
            {
                if (view != null)
                    listViewData.View = view;

                listViewData.ItemsSource = _allitems;
                _initialised = true;

                ConfigureTimer(_timerInterval);
            }

            if (!noFocus) tbFilter.Focus();
        }

        #endregion

        #region Public Properties

        public string Text { get { return tbFilter.Text; } }
        public iSearchItem SelectedItem { get { return _selectedItem; } set { SetSelectedItem(value); } }
        public List<string> SearchFields { get { return _searchFields; } set { _searchFields = value; } }
        public double MaxListHeight { get { return listViewData.MaxHeight; } set { listViewData.MaxHeight = value; } }
        public double MaxListWidth  { get { return listViewData.MaxWidth; } set { listViewData.MaxWidth = value; } }
        public int TextChangeDelay { get { return _textChangedDelay; } set { _textChangedDelay = value; } }
        public bool ValueChanged { get { return _valChanged; } }
      
        public bool AutoFilter 
        { 
            get 
            { 
                return _autoFilter; 
            } 
            set
            { 
                _autoFilter = value;
                ConfigureTimer(_timerInterval);
            } 
        }
        
        public int TimerInterval
        {
            get
            {
                return _timerInterval;
            }
            set
            {
                if (value != _timerInterval)
                    ConfigureTimer(value);
            }
        }

        #endregion

        #region Event Handlers

        void ListSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbFilter.SelectAll();
            if (_selectedItem == _acceptedItem)
                _valChanged = false;
        }

        void ListSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == tbFilter)
            {
                if (listViewData.Visibility == System.Windows.Visibility.Collapsed)
                    Accept();
            }
        }

        void ListSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tbFilter.Height = e.NewSize.Height;
            tbFilter.Width = e.NewSize.Width;
            listGrid.MinWidth = e.NewSize.Width;

            Thickness margin = listGrid.Margin;
            margin.Top = tbFilter.Margin.Top + tbFilter.Height;
            margin.Left = tbFilter.Margin.Left;
            listGrid.Margin = margin;
        }

        private void list_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Use the up arrow key to move up to the text box if on the first item.
            // Use the escape key to return to the text box from anywhere in the list.
            // Use the enter key to accept the currently selected value.
            switch (e.Key)
            {
                case Key.Up:
                    // Move to the filter box if hitting the up arrow when on the first list item
                    if (listViewData.SelectedItem == listViewData.Items[0])
                        tbFilter.Focus();
                    break;
                case Key.Left:
                    SetListVisibility(Visibility.Collapsed);
                    tbFilter.Focus();
                    break;
                case Key.Escape:
                case Key.Tab:
                    if (listViewData.SelectedItem != null)
                    {
                        Accept(listViewData.SelectedItem);
                        tbFilter.Focus();
                    }
                    else
                    {
                        this.Reset();
                    }
                    break;
                case Key.Enter:
                    Accept(listViewData.SelectedItem);
                    tbFilter.Focus();
                    break;
            }
        }

        private void tbFilter_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_initialised)
            {
                int count = 0;

                count = listViewData.Items.Count;

                if (count > 0)
                {
                    // Use the down arrow key and the tab to move to the list and 
                    // select the first item
                    switch (e.Key)
                    {
                        case Key.Down:
                            SetListVisibility(Visibility.Visible);
                            listViewData.Focus();
                            listViewData.SelectedItem = listViewData.Items[0];
                            break;
                        case Key.Tab:
                            if (listViewData.SelectedItem != null && listViewData.Visibility != System.Windows.Visibility.Collapsed)
                            {
                                Accept(listViewData.SelectedItem);
                                tbFilter.Focus();
                            }
                            else
                            {
                                SetListVisibility(Visibility.Collapsed);
                            }
                            break;
                    }
                }
            }
        }

        private void tbFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_initialised)
            {
                // Deselect all items when moving back to the text box
                listViewData.UnselectAll();
            }
        }

        private void OnTimer(object objectState)
        {
            // Get the milliseconds since the last key press
            double millis = TimeDiffMillis(_lastTextChanged);

            if (!_textChanged)
            {
                // Stop the timer if it has been inactive for more than 10 seconds
                if (millis > 4000)
                    StopTimer();
            }
            else
            {
                if (millis > 300)
                {
                    // Stop the timer to do work
                    StopTimer();

                    // Do nothing if the thread is already running
                    if (_thread != null && _thread.IsAlive)
                        return;

                    _thread = new Thread(new ThreadStart(ProcessFilter));
                    _thread.Start();

//                    while (!_thread.IsAlive) ;
                }
            }
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filter = tbFilter.Text;
            _lastTextChanged = DateTime.Now;

            _valChanged = (String.Compare(_filter, (_acceptedItem != null ? _acceptedItem.Value : String.Empty), true) != 0);

            if (_filter == (_acceptedItem == null ? String.Empty : _acceptedItem.Value))
            {
                // Stop the timer processing the list
                _textChanged = false;

                // If the current filter is equal to the starting value then hide the list
                SetListVisibility(System.Windows.Visibility.Collapsed);
            }
            else
            {
                if (_initialised)
                {
                    if (_useTimer)
                    {
                        _textChanged = true;

                        // Start the timer if it was stopped due to inactivity
                        if (!_timerActive)
                            StartTimer();
                    }
                    else
                    {
                        ProcessFilter();
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reset the control to the initial state after data load
        /// </summary>
        public void Reset()
        {
            Init(_allitems, _acceptedItem, _view);
        }

        public void Clear()
        {
            this.Tag = _selectedItem;
            _selectedItem = null;
            SetFilterText(String.Empty);
        }

        public void Dispose()
        {
            if (_filterTimer != null) 
                _filterTimer.Dispose();
        }

        #endregion

        #region Private Methods

        private void ConfigureTimer()
        {
            ConfigureTimer(_timerInterval);
        }

        private void ConfigureTimer(int timerInterval)
        {
            if (_autoFilter)
            {
                _timerInterval = timerInterval;

                if (_timerInterval > 0)
                {
                    if (_filterTimer == null) _filterTimer = new Timer(OnTimer);

                    _useTimer = true;
                    StartTimer();
                }
                else
                {
                    StopTimer();
                    _useTimer = false;
                }
            }
            else
            {
                StopTimer();
                _useTimer = false;
            }
        }

        private void StopTimer()
        {
            if (_filterTimer != null)
            {
                _filterTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _timerActive = false;
            }
        }

        private void StartTimer()
        {
            if (_filterTimer != null && _autoFilter && _useTimer)
            {
                _filterTimer.Change(new TimeSpan(0, 0, 0, 0, _timerInterval), new TimeSpan(0, 0, 0, 0, _timerInterval));
                _timerActive = true;
            }
        }

        /// <summary>
        /// Set the filter text and reset the change flag to stop the list view opening.
        /// The timer is stopped and restarted to avoid the timer firing during the text update.
        /// </summary>
        /// <param name="text"></param>
        private void SetFilterText(string text)
        {
            if (_timerActive) StopTimer();

            tbFilter.Text = text;
            _textChanged = false;

            if (_useTimer) StartTimer();
        }

        /// <summary>
        /// Calculate the total milliseconds between the supplied date time and the current time
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private double TimeDiffMillis(DateTime from)
        {
            TimeSpan ts = DateTime.Now - from;
            return ts.TotalMilliseconds;
        }

        private void ProcessFilter()
        {
            _textChanged = false;

            listViewData.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(delegate()
                {
                    ICollectionView view;

                    if (_filter.Contains(_prevFilter))
                        view = CollectionViewSource.GetDefaultView(listViewData.Items);
                    else
                        view = CollectionViewSource.GetDefaultView(listViewData.ItemsSource);

                    view.Filter = new Predicate<object>(FilterItems);

                    if (listViewData.Items.Count > 0)
                        listViewData.SelectedItem = listViewData.Items[0];
                }));

            SetListVisibility();

            _prevFilter = _filter;
            _lastFilterTime = DateTime.Now;

            if (_useTimer) StartTimer();
        }

        // Compare the iSearchItem with the text. If the item name contains
        // the text return true to add it to the filtered list
        private bool FilterItems(object item)
        {
            bool found = false;

            if (item != null)
            {
                iSearchItem i = (iSearchItem)item;

                string filter = _filter.ToLower().Trim();

                if (filter.Length > 0)
                    found = Find(i, filter);
                else
                    found = true;
            }

            return found;
        }

        private bool Find(iSearchItem item, string text)
        {
            bool found = false;

            if (_searchFields != null)
            {
                // If search fields are defined, loop through them and compare each one
                foreach (string s in _searchFields)
                {
                    // Get the value for the named property specified in the list
                    var propertyInfo = item.GetType().GetProperty(s);
                    var value = propertyInfo.GetValue(item, null);

                    if (value.ToString().ToLower().Contains(text))
                    {
                        found = true;
                        break;
                    }
                }
            }
            else
            {
                found = item.SearchString.ToLower().Contains(text.ToLower());
            }

            return found;
        }

        private void SetListVisibility()
        {
            System.Windows.Visibility v;

            string ival = (_acceptedItem == null ? String.Empty : _acceptedItem.Value);

            if (listViewData.Items.Count == 0 || String.Compare(_filter, ival, true) == 0 || _filter.Trim() == String.Empty) 
                v = System.Windows.Visibility.Collapsed;
            else
                v = System.Windows.Visibility.Visible;

            SetListVisibility(v);
        }

        private void SetListVisibility(System.Windows.Visibility visibility)
        {
            listViewData.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                delegate() 
                { 
                    listViewData.Visibility = visibility;
                    Canvas.SetZIndex(
                        this,
                        (visibility == System.Windows.Visibility.Visible ? (int)123456 : (int)0));
                }));
        }

        private void Accept()
        {
            Accept(null);
        }

        private void Accept(object item)
        {
            if (item != null) SetSelectedItem(item);

            // If the filter differs from the last filter then process the change first
            if (_selectedItem == null || String.Compare(_selectedItem.Value, _filter, true) != 0)
                SetSelectedItem(_filter.Trim() != String.Empty ? FindItemByValue(_filter) : null);
            else
                SetListVisibility(Visibility.Collapsed);

            // Save the item as accepted
            _acceptedItem = _selectedItem;

            if (_valChanged && Changed != null) Changed(this);
        }

        /// <summary>
        /// Use the list view item to update the selected item. Saves the current item to the tag field.
        /// </summary>
        /// <param name="item"></param>
        private void SetSelectedItem(object item)
        {
            string text = "";

            iSearchItem i = (item != null ? (iSearchItem)item : null);

            this.Tag = _selectedItem;
            _selectedItem = i;

            if (_selectedItem != null) text = i.Value;
            else text = _filter;

            SetFilterText(text);

            SetListVisibility(System.Windows.Visibility.Collapsed);
        }

        /// <summary>
        /// Find an item in the complete list based on the name. If there is an exact match then return 
        /// the item, otherwise return a null value.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private iSearchItem FindItemByValue(string Value)
        {
            iSearchItem item = null;

            if (_allitems != null)
            {
                foreach (iSearchItem i in _allitems)
                {
                    if (String.Compare(i.Value, _filter, true) == 0)
                    {
                        item = i;
                        break;
                    }
                }
            }

            return item;
        }

        #endregion
    }
}
