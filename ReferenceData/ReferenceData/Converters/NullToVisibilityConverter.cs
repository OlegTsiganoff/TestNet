using System;
using System.Globalization;
using System.Windows.Data;

namespace ReferenceData
{
    class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
