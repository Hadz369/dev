using System;
using System.Windows;
using System.Windows.Data;

namespace M1.Module.Common.Shared.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            try
            {
                int val = (int)value;
                result = val > 0 ? Visibility.Visible : Visibility.Collapsed;
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
