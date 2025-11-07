
#if IS_WINDOWS
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Node.Net.Converters
{
    /// <summary>
    /// Converts an object to Visibility.Hidden when null, otherwise Visibility.Visible
    /// </summary>
    public class HiddenWhenNull : IValueConverter
    {
        public static HiddenWhenNull Default { get; } = new HiddenWhenNull();

        /// <summary>
        /// Convert
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        /// <summary>
        /// ConvertBack (not implemented)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
#endif