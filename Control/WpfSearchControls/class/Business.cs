using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WpfSearchControls
{
    public class Business
    {
        #region Initialisation

        Data _data = null;
        bool _initialised = false;

        private static readonly Business instance = new Business();

        private Business() { }

        public static Business Instance
        {
            get { return instance; }
        }

        public void Init(string conString)
        {
            _data = new Data(conString);
            _initialised = true;
        }

        #endregion

        #region Properties

        public bool Initialised { get { return _initialised; } }

        #endregion

        #region Public Methods

        public int? GetDefaultCountry()
        {
            return _data.GetDefaultCountry();
        }

        public int? GetDefaultProvince(int? countryId)
        {
            return _data.GetDefaultProvince(countryId);
        }

        public List<iSearchItem> GetCountrySearchList()
        {
            DataTable dt = _data.GetCountries();

            // Convert the table to a list
            List<iSearchItem> list = new List<iSearchItem>();
            foreach (DataRow r in dt.Rows)
            {
                SearchItem i = new SearchItem((int)r[0], r[1].ToString());
                list.Add(i);
            }
         
            return list;
        }

        public List<iSearchItem> GetProvinceSearchList(int countryId)
        {
            DataTable dt = _data.GetProvinces(countryId);

            // Convert the table to a list
            List<iSearchItem> list = new List<iSearchItem>();
            foreach (DataRow r in dt.Rows)
            {
                SearchItem i = new SearchItem((int)r["ProvincePk"], r["Province"].ToString());
                list.Add(i);
            }

            return list;
        }

        public List<iSearchItem> GetDistrictSearchList(int provinceId)
        {
            DataTable dt = _data.GetDistricts(provinceId);

            // Convert the table to a list
            List<iSearchItem> list = new List<iSearchItem>();
            foreach (DataRow r in dt.Rows)
            {
                Suburb i = new Suburb((int)r["DistrictPk"], r["Suburb"].ToString(), r["PostCode"].ToString());
                list.Add(i);
            }

            return list;
        }

        public List<iSearchItem> GetAddressTypeSearchList()
        {
            return GetAddressTypeSearchList(null);
        }

        public List<iSearchItem> GetAddressTypeSearchList(List<int> availableAddressTypes)
        {
            DataTable dt = _data.GetAddressTypes();

            SearchItem i = null;

            // Convert the table to a list
            List<iSearchItem> list = new List<iSearchItem>();

            foreach (DataRow r in dt.Rows)
            {
                if (availableAddressTypes != null)
                {
                    foreach (int type in availableAddressTypes)
                    {
                        if ((int)r[0] == type)
                        {
                            i = new SearchItem((int)r["CodeSuidPk"], r["Value"].ToString());
                            break;
                        }
                    }
                    list.Add(i);
                }
                else
                {
                    i = new SearchItem((int)r["CodeSuidPk"], r["Value"].ToString());
                    list.Add(i);
                }
            }

            return list;
        }

        public List<iSearchItem> GetStreetTypeSearchList()
        {
            DataTable dt = _data.GetStreetTypes();

            // Convert the table to a list
            List<iSearchItem> list = new List<iSearchItem>();
            
            foreach (DataRow r in dt.Rows)
            {
                StreetType i = new StreetType((int)r["StreetPk"], r["Name"].ToString(), r["Abbreviation"].ToString());
                list.Add(i);
            }

            return list;
        }

        #endregion
    }
}
