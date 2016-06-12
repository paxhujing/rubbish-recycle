using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RubbishRecycle.PC.Main.Converters
{
    public class TokenToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String token = value as String;
            String arg = parameter as String;
            if(arg == "revert")
            {
                return String.IsNullOrWhiteSpace(token) ? Visibility.Visible : Visibility.Collapsed;
            }
            return String.IsNullOrWhiteSpace(token) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
