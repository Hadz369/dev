using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TabControlTest
{
    public enum ReportParameterType
    {
        String,
        MultiLineString,
        Numeric,
        Date,
        Time,
        Combo
    }

    public class ReportParameterViewModel : ViewModelBase
    {
        object _initialValue = null;
        object _value;

        bool _isValid = true;

        public ReportParameterViewModel(ReportParameterType parameterType, string key, string name) 
            : this(parameterType, key, name, null, 0) { }

        public ReportParameterViewModel(ReportParameterType parameterType, string key, string name, int maxLength)
            : this(parameterType, key, name, null, maxLength) { }

        public ReportParameterViewModel(ReportParameterType parameterType, string key, string name, object defaultValue)
            : this(parameterType, key, name, defaultValue, 0) { }

        public ReportParameterViewModel(ReportParameterType parameterType, string key, string name, object defaultValue, int maxLength)
        {
            ParameterType = parameterType;
            Key = key;
            Name = name;
            _value = _initialValue = defaultValue;
            MaxLength = maxLength;
        }

        public ReportParameterType ParameterType { get; private set; }

        public string Key { get; private set; }

        public string Name { get; private set; }

        public int MaxLength { get; private set; }

        public bool IsChanged { get { return !_initialValue.Equals(_value); } }

        public bool IsValid { get { return _isValid; } }

        public string ValidationMessage { get; set; }

        public object Value 
        { 
            get  { return _value; }
            set { SetValue(value); }
        }

        void SetValue(object value)
        {
            _value = value;
            OnPropertyChanged("Value");
        }

        /// <summary>
        /// Convert the parameter value to a type
        /// </summary>
        /// <typeparam name="T">The data type to convert the value to</typeparam>
        /// <param name="value">The converted value</param>
        /// <returns>
        /// True  = Convert successful
        /// False = Convert failed
        /// </returns>
        public bool GetValue<T>(out T value)
        {
            bool success = false;
            value = default(T);

            try
            { 
                value = (T)Convert.ChangeType(_value, typeof(T));
                success = true;
            }
            catch { }

            return success;
        }
    }

    public class ReportParameterGroup
    {
        public ReportParameterGroup(string name)
        {
            Name = name;
            Parameters = new ObservableCollection<ReportParameterViewModel>();
        }

        public string Name { get; private set; }
        public ObservableCollection<ReportParameterViewModel> Parameters { get; private set; }
    }

    public class ReportParameterGroups : List<ObservableCollection<ReportParameterViewModel>> { }
}
