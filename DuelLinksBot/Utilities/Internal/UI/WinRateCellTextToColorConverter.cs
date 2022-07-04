using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using DuelLinksBot.Utilities.Internal.Extensions;

namespace DuelLinksBot.Utilities.Internal.UI
{
	public class WinRateCellTextToColorConverter : IValueConverter
	{
		private static readonly string DEFAULT_COLOR = Color.Black.ToHexString();
		private static readonly string VERY_BAD_WINRATE_COLOR = Color.DarkRed.ToHexString();
		private static readonly string BAD_WINRATE_COLOR = Color.Red.ToHexString();
		private static readonly string AVG_WINRATE_COLOR = Color.Orange.ToHexString();
		private static readonly string GOOD_WINRATE_COLOR = Color.Green.ToHexString();
		private static readonly string BEST_WINRATE_COLOR = Color.DarkGreen.ToHexString();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is decimal decValue))
			{
				return DEFAULT_COLOR;
			}

			try
			{
				var intValue = System.Convert.ToInt32(Math.Round(decValue));
				if (intValue < 20)
				{
					return VERY_BAD_WINRATE_COLOR;
				}

				if (intValue < 40)
				{
					return BAD_WINRATE_COLOR;
				}

				if (intValue > 90)
				{
					return BEST_WINRATE_COLOR;
				}

				if (intValue > 80)
				{
					return GOOD_WINRATE_COLOR;
				}

				return AVG_WINRATE_COLOR;
			}
			catch (FormatException)
			{
				return DEFAULT_COLOR;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("Not possible to convert back.");
		}
	}
}