using System;
using System.Windows.Data;
using System.Windows;

namespace QuikTODO
{
    public class InverseBoolVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a Boolean input from "True" to "Collapsed" and "False" to "Visible".
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
