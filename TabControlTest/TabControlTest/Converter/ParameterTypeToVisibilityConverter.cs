using System;
using System.Windows;
using System.Windows.Data;

namespace TabControlTest
{
    public class ParameterTypeToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            try
            {
                if (value is ReportParameterType && parameter is ReportParameterType)
                {
                    var val1 = (ReportParameterType)value;
                    var val2 = (ReportParameterType)parameter;

                    if (val1 == val2)
                        result = Visibility.Visible;
                }
            }
            catch { }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
