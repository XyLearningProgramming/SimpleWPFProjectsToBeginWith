﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace RichTextEditor.Utils.Converters
{
	class BooleanToVisibilityConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
