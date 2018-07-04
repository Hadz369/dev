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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace WpfSearchControls
{
    /// <summary>
    /// Interaction logic for AddressEditor.xaml
    /// </summary>
    public partial class AddressEditor : UserControl
    {
        #region Initialisation

        Business _business;
        Address _address;

        int? _defCountry = null, _defState = null;

        // This list is set here for use with street name comparisons
        List<iSearchItem> _streetTypes = new List<iSearchItem>();

        public AddressEditor()
        {
            InitializeComponent();

            lsCountry.Changed += ListSearch_OnChanged;
            lsProvince.Changed += ListSearch_OnChanged;
            lsDistrict.Changed += ListSearch_OnChanged;
            tbStreetName.LostFocus += tbStreetName_LostFocus;
        }

        public void Init(Address address)
        {
            Init(address,  null);
        }

        public void Init(Address address, List<int> availableAddressTypes)
        {
            _business = Business.Instance;

            if (!_business.Initialised)
                throw new Exception("Attempt to instance uniitialised business class in the AddressEditor user control.");

            if (availableAddressTypes != null && availableAddressTypes.Count == 0)
                throw new Exception("The available type list must contain at least one value when specified.");

            _address = address;

            _defCountry = _business.GetDefaultCountry();
            
            if (_defCountry != null)
                _defState = _business.GetDefaultProvince(_defCountry);

            LoadCountryList();
            if (lsCountry.SelectedItem != null)
            {
                LoadProvinceList(lsCountry.SelectedItem.Id);
                if (lsProvince.SelectedItem != null)
                {
                    LoadDistrictList(lsProvince.SelectedItem.Id);
                }
            }

            LoadAddressTypes(availableAddressTypes);
            LoadStreetTypes();

            lsDistrict.Focus();
        }

        #endregion

        #region Properties

        #endregion

        #region Event Handlers

        void tbStreetName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.Compare((tbStreetName.Tag == null ? String.Empty : tbStreetName.Tag.ToString()), tbStreetName.Text, true) != 0)
            {
                if (tbStreetName.Text.Trim() != String.Empty)
                {
                    int wordlen = 0;

                    iSearchItem item = FindStreetTypeByValue(tbStreetName.Text, ref wordlen);
                    if (item != null)
                    {
                        lsStreetType.SelectedItem = item;
                        _address.StreetType = item;

                        tbStreetName.Text = tbStreetName.Text.Substring(0, tbStreetName.Text.Length - wordlen).Trim();
                        tbStreetName.Tag = tbStreetName.Text;
                    }
                    else
                    {
                        lsStreetType.Clear();
                        _address.StreetType = null;
                    }
                    _address.StreetName = tbStreetName.Text;
                }
                else
                {
                    lsStreetType.Clear();
                    _address.StreetName = String.Empty;
                }
            }
        }

        void ListSearch_OnChanged(object sender)
        {
            ListSearch ls = (ListSearch)sender;

            if (ls.ValueChanged)
            {
                if (ls.Name == lsCountry.Name)
                {
                    lsProvince.Init();
                    lsDistrict.Init();

                    if (ls.SelectedItem != null)
                    {
                        _address.Country = ls.SelectedItem;
                        LoadProvinceList(ls.SelectedItem.Id);
                        if (lsProvince.SelectedItem != null)
                            LoadDistrictList(lsProvince.SelectedItem.Id);
                    }
                    else
                    {
                        _address.Country = null;
                        lsProvince.Init();
                        lsDistrict.Init();
                        lsCountry.Focus();
                    }
                }
                else if (ls.Name == lsProvince.Name)
                {
                    lsDistrict.Init();
                    tbPostCode.Clear();

                    if (lsProvince.SelectedItem != null)
                    {
                        _address.Province = ls.SelectedItem;
                        LoadDistrictList(lsProvince.SelectedItem.Id);
                    }
                    else
                    {
                        _address.Province = null;
                        lsDistrict.Init();
                        lsProvince.Focus();
                    }
                }
                else if (ls.Name == lsDistrict.Name)
                {
                    tbPostCode.Clear();
                    tbPostCode.IsEnabled = true;

                    if (lsDistrict.SelectedItem != null)
                    {
                        _address.District = ls.SelectedItem;
                        tbPostCode.Text = ((Suburb)lsDistrict.SelectedItem).PostCode;
                        tbPostCode.IsEnabled = false;
                    }
                    else
                    {
                        _address.District = null;
                        tbPostCode.IsEnabled = true;
                        tbPostCode.Focus();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private iSearchItem FindStreetTypeByValue(string value, ref int wordlen)
        {
            iSearchItem item = null;

            int x = value.Trim().LastIndexOf(' ');
            string lastword = String.Empty;

            if (x > 0)
                lastword = value.Substring(x).Trim().ToLower();

            foreach (iSearchItem i in _streetTypes)
            {
                StreetType st = (StreetType)i;
                if (st.Abbreviation.Trim() == String.Empty)
                {
                    continue;
                }
                else if (String.Compare(st.Abbreviation, lastword, true) == 0)
                {
                    item = i;
                    wordlen = lastword.Length;
                    break;
                }
            }

            if (item == null)
            {
                foreach (iSearchItem i in _streetTypes)
                {
                    if (String.Compare(i.Value, lastword, true) == 0)
                    {
                        item = i;
                        wordlen = lastword.Length;
                        break;
                    }
                }
            }
            return item;
        }

        private void ProcessChangedItem(ListSearch ls, iSearchItem item)
        {
            if (ls == lsCountry) LoadProvinceList(item.Id);
            else if (ls == lsProvince) LoadDistrictList(item.Id);
            else tbPostCode.Text = ((Suburb)item).PostCode;
        }

        private void LoadCountryList()
        {
            // Placeholder for the default value if found
            iSearchItem defval = null;

            // Get the search list
            List<iSearchItem> list = _business.GetCountrySearchList();

            // Locate the default if it exists
            foreach (iSearchItem i in list)
            {
                if (i.Id == _defCountry)
                    defval = i;
            }

            lsCountry.Init(list, defval);
            lsCountry.Focus();
        }

        private void LoadProvinceList(int countryId)
        {
            // Placeholder for the default value if found
            iSearchItem defval = null;

            // Get the search list
            List<iSearchItem> list = _business.GetProvinceSearchList(countryId);
            
            // Locate the default if it exists
            foreach (iSearchItem i in list)
            {
                if (i.Id == _defState)
                    defval = i;
            }

            lsProvince.Init(list, defval);
            lsProvince.Focus();
        }

        private void LoadDistrictList(int provinceId)
        {
            // Get the search list
            List<iSearchItem> list = _business.GetDistrictSearchList(provinceId);

            GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            myGridView.ColumnHeaderToolTip = "Select a suburb";

            Style style = new Style(typeof(GridViewColumnHeader));
            style.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));

            GridViewColumn gvc;

            gvc = new GridViewColumn();
            gvc.DisplayMemberBinding = new Binding("PostCode");
            gvc.Header = "PostCode";
            gvc.Width = 60;
            gvc.HeaderContainerStyle = style;
            myGridView.Columns.Add(gvc);

            gvc = new GridViewColumn();
            gvc.DisplayMemberBinding = new Binding("Value");
            gvc.Header = "Suburb";
            gvc.Width = 120;
            gvc.HeaderContainerStyle = style;
            myGridView.Columns.Add(gvc);

            lsDistrict.Init(list, null, myGridView);
            lsDistrict.Focus();
        }

        private void LoadAddressTypes(List<int> availableAddressTypes)
        {
            // Get the search list
            List<iSearchItem> list = _business.GetAddressTypeSearchList(availableAddressTypes);

            if (list.Count > 0)
            {
                comboAddrType.ItemsSource = list;
                comboAddrType.SelectedIndex = 0;
            }
        }

        private void LoadStreetTypes()
        {
            // Get the search list
            _streetTypes = _business.GetStreetTypeSearchList();

            GridView myGridView = new GridView();
            myGridView.AllowsColumnReorder = true;
            myGridView.ColumnHeaderToolTip = "Select a street type";

            Style style = new Style(typeof(GridViewColumnHeader));
            style.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));

            GridViewColumn gvc;

            gvc = new GridViewColumn();
            gvc.DisplayMemberBinding = new Binding("Value");
            gvc.Header = "Street Type";
            gvc.Width = 120;
            gvc.HeaderContainerStyle = style;
            myGridView.Columns.Add(gvc);

            gvc = new GridViewColumn();
            gvc.DisplayMemberBinding = new Binding("Abbreviation");
            gvc.Header = "Abbrev";
            gvc.Width = 120;
            gvc.HeaderContainerStyle = style;
            myGridView.Columns.Add(gvc);

            lsStreetType.Init(_streetTypes, null, myGridView, true);
        }

        #endregion
    }
}
