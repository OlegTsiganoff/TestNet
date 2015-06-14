using System;
using System.Globalization;
using System.Windows.Data;

namespace ReferenceData
{
    class TextLengthToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (s == null || s.Equals(String.Empty))
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
