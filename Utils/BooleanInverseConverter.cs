using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace BombDropper.Utils
{
	public class BooleanInverseConverter: IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? false : true;
        }
    }
}
