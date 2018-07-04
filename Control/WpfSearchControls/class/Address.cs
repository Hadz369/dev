using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WpfSearchControls
{
    public class Address : INotifyPropertyChanged
    {
        iSearchItem _country, _province, _district, _addressType, _streetType;
        string _desc, _streetno, _streetname;
        bool _isValid = false;

        private event PropertyChangedEventHandler LocalPropertyChanged;
        public event  PropertyChangedEventHandler PropertyChanged;

        public Address()
        {
            this.LocalPropertyChanged += Address_PropertyChanged;
        }

        public void Address_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.Country == null ||
                this.Province == null ||
                this.District == null ||
                this.StreetName == String.Empty)
                _isValid = false;
            else
                _isValid = true;

            // Send the event out
            if (PropertyChanged != null) PropertyChanged(sender, e);
        }

        public bool IsValid { get { return _isValid; } }
        public iSearchItem Country { get { return _country; } set { _country = value; } }
        public iSearchItem Province { get { return _province; } set { _province = value; } }
        public iSearchItem District { get { return _district; } set { _district = value; } }
        public iSearchItem AddressType { get { return _addressType; } set { _addressType = value; } }
        public iSearchItem StreetType { get { return _streetType; } set { _streetType = value; } }
        public string Description { get { return _desc; } set { _desc = value; } }
        public string StreetNo { get { return _streetno; } set { _streetno = value; } }
        public string StreetName { get { return _streetname; } set { _streetname = value; } }

        private void Validate()
        {
        }
    }
}
