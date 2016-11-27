using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RandomSongPicker.Converters
{
	public class InvertedBooleanToVisibilityConverter : IValueConverter
	{
		#region Implementation of IValueConverter

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var c = new BooleanToVisibilityConverter();
			var vis = (Visibility)c.Convert(value, targetType, parameter, culture);

			if (vis == Visibility.Visible)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var c = new BooleanToVisibilityConverter();
			var vis = (bool)c.ConvertBack(value, targetType, parameter, culture);

			return !vis;
		}

		#endregion
	}
}
