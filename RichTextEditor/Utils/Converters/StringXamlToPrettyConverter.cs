using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Xml.Linq;

namespace RichTextEditor.Utils.Converters
{
	/// <summary>
	/// Make raw xaml prettier
	/// </summary>
	class StringXamlToPrettyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is String str)
			{
				if(string.IsNullOrEmpty(str)) return str;
				try
				{
					str = XDocument.Parse(str).ToString();
				}
				catch(Exception e)
				{
					// do nothing, leave this to helper class
				}
				return str;

			} else
				throw new ArgumentException(nameof(value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is String str)
			{
				if(string.IsNullOrEmpty(str)) return str;
				try
				{
					str = XDocument.Parse(str, LoadOptions.None).ToString(SaveOptions.DisableFormatting);
				}
				catch(Exception e)
				{
					// do nothing, leave this to helper class
				}
				return str;
			}
			else
				throw new ArgumentException(nameof(value));
		}
	}
}
