using System;

namespace DuelLinksBot.HelpersLib.Native
{
	public static class Helpers
	{
		private static readonly Version OsVersion = Environment.OSVersion.Version;

		public static bool IsWindowsVistaOrGreater()
		{
			return OsVersion.Major >= 6;
		}

		public static bool IsWindows10OrGreater()
		{
			return OsVersion.Major >= 10;
		}
	}
}