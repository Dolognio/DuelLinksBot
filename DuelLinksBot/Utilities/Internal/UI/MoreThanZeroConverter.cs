using System;
using System.Globalization;
using System.Windows.Data;

namespace DuelLinksBot.Utilities.Internal.UI
{
	public class MoreThanZeroConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if (value == null || !(value is string s))
			{
				return false;
			}

			try
			{
				var intValue = System.Convert.ToInt32(s);
				return intValue > 0;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			throw new Exception("Not possible to convert back.");
		}
	}
}