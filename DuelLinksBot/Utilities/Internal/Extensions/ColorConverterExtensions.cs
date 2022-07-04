using System.Drawing;

namespace DuelLinksBot.Utilities.Internal.Extensions
{
	public static class ColorConverterExtensions
	{
		public static string ToHexString(this Color c)
		{
			return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
		}
	}
}